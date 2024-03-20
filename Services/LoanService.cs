using Finals.Contexts;
using Finals.Dtos;
using Finals.Enums;
using Finals.Helpers;
using Finals.Interfaces;
using Finals.Models;
using Microsoft.EntityFrameworkCore;

namespace Finals.Services;

public class LoanService : ILoanService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<LoanService> _logger;
    private readonly GetUserFromContext _getUserFromContext;

    public LoanService(ApplicationDbContext dbContext,
        ILogger<LoanService> logger, GetUserFromContext getUserFromContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _getUserFromContext = getUserFromContext;
    }

    public async Task<Loan> CreateLoan(LoanDto loanDto)
    {
        var user = await _getUserFromContext.GetUser();
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        if (user.IsBlocked)
        {
            throw new InvalidOperationException("User is blocked.");
        }

        decimal maxAmount = 0;
        switch (loanDto.LoanPeriod)
        {
            case LoanPeriod.HalfYear:
            case LoanPeriod.OneYear:
                if (user.Salary >= 1000 && user.Salary < 1500)
                {
                    maxAmount = 2000;
                }
                else if (user.Salary >= 1500)
                {
                    maxAmount = 2000;
                }

                break;
            case LoanPeriod.TwoYears:
            case LoanPeriod.FiveYears:
                if (user.Salary >= 1500 && user.Salary < 3500)
                {
                    maxAmount = 7500;
                }
                else if (user.Salary >= 3500)
                {
                    maxAmount = 7500;
                }

                break;
            case LoanPeriod.TenYears:
                if (user.Salary >= 3500)
                {
                    maxAmount = 20000;
                }

                break;
            default:
                throw new ArgumentException("Invalid loan type.");
        }

        if (loanDto.RequestedAmount > maxAmount)
        {
            throw new InvalidOperationException(
                $"Requested loan amount exceeds the maximum allowed amount ({maxAmount}).");
        }

        var loan = new Loan
        {
            RequstedAmount = loanDto.RequestedAmount,
            FinalAmount = loanDto.RequestedAmount +
                          (loanDto.RequestedAmount * ((int)loanDto.LoanPeriod / 100)),
            LoanPeriod = loanDto.LoanPeriod,
            LoanCurrency = loanDto.LoanCurrency,
            LoanType = loanDto.LoanType,
            ApplicationUser = user
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();

        return loan;
    }

    public async Task<LoanDto> GetLoan(int id)
    {
        var user = await _getUserFromContext.GetUser();
        if (user == null)
        {
            return null;
        }

        var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id && l.ApplicationUserId == user.Id);
        if (loan == null)
        {
            return null;
        }

        var loanDto = new LoanDto
        {
            RequestedAmount = loan.RequstedAmount,
            FinalAmount = loan.FinalAmount,
            LoanPeriod = loan.LoanPeriod,
            LoanType = loan.LoanType,
            LoanCurrency = loan.LoanCurrency,
            LoanStatus = loan.LoanStatus
        };

        return loanDto;
    }

    public async Task<LoanDtos> GetAllLoans()
    {
        var user = await _getUserFromContext.GetUser();
        if (user == null)
        {
            return null;
        }

        IQueryable<Loan> loansFromDb = Enumerable.Empty<Loan>().AsQueryable();

        if (user.Role == Role.Accountant)
        {
            loansFromDb = _dbContext.Loans.AsQueryable();
        }
        else if (user.Role == Role.Customer)
        {
            loansFromDb = _dbContext.Loans.Where(l => l.ApplicationUserId == user.Id).AsQueryable();
        }

        var loansDto = new LoanDtos();

        if (loansFromDb == null || !loansFromDb.Any())
        {
            return loansDto;
        }

        foreach (var loan in loansFromDb)
        {
            var loanDto = new LoanDto
            {
                RequestedAmount = loan.RequstedAmount,
                FinalAmount = loan.FinalAmount,
                LoanPeriod = loan.LoanPeriod,
                LoanType = loan.LoanType,
                LoanCurrency = loan.LoanCurrency,
                LoanStatus = loan.LoanStatus
            };

            loansDto.Loans.Add(loanDto);
        }

        return loansDto;
    }

    public async Task<bool> DeleteLoan(int id)
    {
        var user = await _getUserFromContext.GetUser();
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id);
        if (loan == null)
        {
            throw new InvalidOperationException("Loan not found.");
        }

        if (user.Role == Role.Customer && (loan.ApplicationUserId != user.Id || loan.LoanStatus != LoanStatus.PENDING))
        {
            return false;
        }

        if (user.Role == Role.Accountant || loan.LoanStatus == LoanStatus.PENDING)
        {
            _dbContext.Loans.Remove(loan);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<LoanDto> ModifyLoan(int id, LoanDto loanDto)
    {
        var user = await _getUserFromContext.GetUser();
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id);
        if (loan == null)
        {
            throw new InvalidOperationException("Loan not found.");
        }

        if (user.Role == Role.Customer && (loan.ApplicationUserId != user.Id || loan.LoanStatus != LoanStatus.PENDING))
        {
            throw new InvalidOperationException("Operation not allowed.");
        }

        if (user.Role == Role.Accountant || loan.LoanStatus == LoanStatus.PENDING)
        {
            loan.RequstedAmount = loanDto.RequestedAmount;
            loan.FinalAmount = loanDto.RequestedAmount +
                               (loanDto.RequestedAmount * ((int)loanDto.LoanPeriod / 100));
            loan.LoanPeriod = loanDto.LoanPeriod;
            loan.LoanCurrency = loanDto.LoanCurrency;
            loan.LoanType = loanDto.LoanType;
            await _dbContext.SaveChangesAsync();
            return loanDto;
        }

        throw new InvalidOperationException("Operation not allowed.");
    }

    public async Task<bool> AcceptLoan(int id)
    {
        var user = await _getUserFromContext.GetUser();
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        if (user.Role != Role.Accountant)
        {
            throw new UnauthorizedAccessException("Only users with the Accountant role can accept loans.");
        }

        var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id);
        if (loan == null)
        {
            throw new InvalidOperationException("Loan not found.");
        }

        if (loan.LoanStatus != LoanStatus.PENDING)
        {
            throw new InvalidOperationException("Loan is not in a PENDING status.");
        }

        loan.LoanStatus = LoanStatus.ACCEPTED;
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeclineLoan(int id)
    {
        var user = await _getUserFromContext.GetUser();
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        if (user.Role != Role.Accountant)
        {
            throw new UnauthorizedAccessException("Only users with the Accountant role can decline loans.");
        }

        var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id);
        if (loan == null)
        {
            throw new InvalidOperationException("Loan not found.");
        }

        if (loan.LoanStatus != LoanStatus.PENDING)
        {
            throw new InvalidOperationException("Loan is not in a PENDING status.");
        }

        loan.LoanStatus = LoanStatus.DECLINED;
        await _dbContext.SaveChangesAsync();

        return true;
    }
}