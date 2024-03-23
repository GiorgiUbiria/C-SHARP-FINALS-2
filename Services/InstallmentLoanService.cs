using Finals.Contexts;
using Finals.Dtos;
using Finals.Enums;
using Finals.Helpers;
using Finals.Interfaces;
using Finals.Models;

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
            LoanCurrency = loanDto.LoanCurrency,
            LoanType = LoanType.INSTALLMENT,
            LoanStatus = LoanStatus.PENDING,
            ApplicationUser = user,
            ProductId = loanDto.ProductId,
            Product = product
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();

        return loan;
    }
}