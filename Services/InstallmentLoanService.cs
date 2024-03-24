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

        if (user.Salary < product.Price * 3)
        {
            throw new InvalidOperationException("User's salary is insufficient to afford the selected product.");
        }

        var loan = new Loan
        {
            RequstedAmount = (int)product.Price,
            FinalAmount = (int)product.Price,
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