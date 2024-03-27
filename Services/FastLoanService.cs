using Finals.Contexts;
using Finals.Dtos;
using Finals.Enums;
using Finals.Helpers;
using Finals.Interfaces;
using Finals.Models;
using Microsoft.EntityFrameworkCore;

namespace Finals.Services;

public class FastLoanService : IFastLoanService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<LoanService> _logger;
    private readonly GetUserFromContext _getUserFromContext;

    public FastLoanService(ApplicationDbContext dbContext,
        ILogger<LoanService> logger, GetUserFromContext getUserFromContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _getUserFromContext = getUserFromContext;
    }

    public async Task<Loan> CreateFastLoan(FastLoanRequestDto fastLoanDto)
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
        switch (fastLoanDto.LoanPeriod)
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

        if (fastLoanDto.RequestedAmount > maxAmount)
        {
            throw new InvalidOperationException(
                $"Requested loan amount exceeds the maximum allowed amount ({maxAmount}).");
        }

        var loan = new Loan
        {
            RequstedAmount = fastLoanDto.RequestedAmount,
            FinalAmount = Helpers.LoanCalculator.CalculateFinalAmount(fastLoanDto.RequestedAmount, fastLoanDto.LoanPeriod),
            AmountLeft = Helpers.LoanCalculator.CalculateFinalAmount(fastLoanDto.RequestedAmount, fastLoanDto.LoanPeriod),
            LoanPeriod = fastLoanDto.LoanPeriod,
            LoanCurrency = fastLoanDto.LoanCurrency,
            LoanType = fastLoanDto.LoanType,
            LoanStatus = LoanStatus.PENDING,
            ProductId = null,
            Product = null,
            CarId = null,
            Car = null,
            ApplicationUser = user,
            ApplicationUserId = user.Id,
            ApplicationUserEmail = user?.Email
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();

        return loan;
    }

    public async Task<Loan> ModifyFastLoan(int id, FastLoanRequestDto fastLoanDto)
    {
        try
        {
            _logger.LogInformation("Attempting to modify loan with ID: {LoanId}", id);

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                throw new InvalidOperationException("User not found.");
            }

            var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id);
            if (loan == null)
            {
                _logger.LogInformation("Loan not found.");
                throw new InvalidOperationException("Loan not found.");
            }

            if (user.Role == Role.Customer &&
                (loan.ApplicationUserId != user.Id || loan.LoanStatus != LoanStatus.PENDING))
            {
                _logger.LogInformation("Operation not allowed.");
                throw new InvalidOperationException("Operation not allowed.");
            }

            if (user.Role == Role.Accountant || loan.LoanStatus == LoanStatus.PENDING)
            {
                loan.RequstedAmount = fastLoanDto.RequestedAmount;
                loan.FinalAmount = Helpers.LoanCalculator.CalculateFinalAmount(fastLoanDto.RequestedAmount, fastLoanDto.LoanPeriod);
                loan.AmountLeft = Helpers.LoanCalculator.CalculateFinalAmount(fastLoanDto.RequestedAmount, fastLoanDto.LoanPeriod);
                loan.LoanPeriod = fastLoanDto.LoanPeriod;
                loan.LoanCurrency = fastLoanDto.LoanCurrency;
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Loan modified successfully.");
                return loan;
            }

            _logger.LogInformation("Operation not allowed.");
            throw new InvalidOperationException("Operation not allowed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while modifying loan with ID: {LoanId}. Error: {ErrorMessage}", id,
                ex.Message);
            throw;
        }
    }

    public async Task<FastLoanDto> GetFastLoan(int id)
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve loan with ID: {LoanId}", id);

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                return null;
            }

            var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id && l.ApplicationUserId == user.Id);
            if (loan == null)
            {
                _logger.LogInformation("Loan not found.");
                return null;
            }

            var loanDto = new FastLoanDto
            {
                Id = loan.Id,
                RequestedAmount = loan.RequstedAmount,
                FinalAmount = loan.FinalAmount,
                AmountLeft = loan.AmountLeft,
                LoanPeriod = loan.LoanPeriod,
                LoanType = loan.LoanType,
                LoanCurrency = loan.LoanCurrency,
                LoanStatus = loan.LoanStatus
            };

            _logger.LogInformation("Loan with ID {LoanId} retrieved successfully.", id);
            return loanDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving loan with ID: {LoanId}. Error: {ErrorMessage}", id,
                ex.Message);
            throw;
        }
    }
}