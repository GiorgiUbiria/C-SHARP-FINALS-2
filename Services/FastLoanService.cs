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

    public async Task<Loan> CreateFastLoan(LoanRequestDto loanDto)
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
            LoanStatus = LoanStatus.PENDING,
            ProductId = null,
            Product = null,
            ApplicationUser = user
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();

        return loan;
    }
    
    public async Task<LoanDto> GetFastLoan(int id)
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

            var loanDto = new LoanDto
            {
                RequestedAmount = loan.RequstedAmount,
                FinalAmount = loan.FinalAmount,
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