using Finals.Contexts;
using Finals.Dtos;
using Finals.Enums;
using Finals.Helpers;
using Finals.Interfaces;
using Finals.Models;
using Microsoft.EntityFrameworkCore;

namespace Finals.Services;

public class InstallmentLoanService : IInstallmentLoanService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<LoanService> _logger;
    private readonly GetUserFromContext _getUserFromContext;

    public InstallmentLoanService(ApplicationDbContext dbContext,
        ILogger<LoanService> logger, GetUserFromContext getUserFromContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _getUserFromContext = getUserFromContext;
    }

    public async Task<Loan> CreateInstallmentLoan(InstallmentLoanRequestDto loanDto)
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

        var product = await _dbContext.Products.FindAsync(loanDto.ProductId);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        if (loanDto.LoanPeriod == LoanPeriod.FiveYears || loanDto.LoanPeriod == LoanPeriod.TenYears)
        {
            throw new InvalidOperationException("Installment loan can't be longer than two years.");
        }

        if (user.Salary < product.Price * 3)
        {
            throw new InvalidOperationException("User's salary is insufficient to afford the selected product.");
        }

        var loan = new Loan
        {
            RequstedAmount = (int)product.Price,
            FinalAmount = Helpers.LoanCalculator.CalculateFinalAmount((int)product.Price, loanDto.LoanPeriod),
            LoanPeriod = loanDto.LoanPeriod,
            ProductId = loanDto.ProductId,
            LoanCurrency = Currency.GEL,
            LoanType = LoanType.INSTALLMENT,
            LoanStatus = LoanStatus.PENDING,
            Product = product,
            ApplicationUser = user
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();

        return loan;
    }

    public async Task<Loan> ModifyInstallmentLoan(int id, InstallmentLoanRequestDto installmentLoanDto)
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

            if (loan.LoanType != LoanType.INSTALLMENT)
            {
                _logger.LogInformation("Cannot modify loans that are not of type Installment.");
                throw new InvalidOperationException("Cannot modify loans that are not of type Installment.");
            }

            if (user.Role == Role.Customer &&
                (loan.ApplicationUserId != user.Id || loan.LoanStatus != LoanStatus.PENDING))
            {
                _logger.LogInformation("Operation not allowed.");
                throw new InvalidOperationException("Operation not allowed.");
            }

            var product = await _dbContext.Products.FindAsync(installmentLoanDto.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            if (user.Role == Role.Accountant || loan.LoanStatus == LoanStatus.PENDING)
            {
                loan.LoanPeriod = installmentLoanDto.LoanPeriod;
                loan.ProductId = installmentLoanDto.ProductId;
                loan.RequstedAmount = (int)product.Price;
                loan.FinalAmount = Helpers.LoanCalculator.CalculateFinalAmount((int)product.Price, installmentLoanDto.LoanPeriod);
                loan.Product = product;
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

    public async Task<InstallmentLoanDto> GetLoan(int id)
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

            var loanDto = new InstallmentLoanDto()
            {
                Id = loan.Id,
                RequestedAmount = loan.RequstedAmount,
                FinalAmount = loan.FinalAmount,
                LoanPeriod = loan.LoanPeriod,
                LoanType = loan.LoanType,
                LoanCurrency = loan.LoanCurrency,
                LoanStatus = loan.LoanStatus,
                ProductId = (int)loan.ProductId,
                Product = loan?.Product
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