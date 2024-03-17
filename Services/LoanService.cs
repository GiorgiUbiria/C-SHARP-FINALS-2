using Finals.Contexts;
using Finals.Dtos;
using Finals.Enums;
using Finals.Interfaces;
using Finals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Finals.Services;

public class LoanService : ILoanService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoanService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<Loan> CreateLoan(LoanDto loanDto)
    {
        var user = await GetUserFromHttpContext();
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        if (user.IsBlocked)
        {
            throw new InvalidOperationException("User is blocked.");
        }

        var loan = new Loan
        {
            Amount = loanDto.Amount,
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
        var user = await GetUserFromHttpContext();
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
            Amount = loan.Amount,
            LoanPeriod = loan.LoanPeriod,
            LoanType = loan.LoanType,
            LoanCurrency = loan.LoanCurrency
        };

        return loanDto;
    }

    public async Task<LoanDtos> GetAllLoans()
    {
        var user = await GetUserFromHttpContext();
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
                Amount = loan.Amount,
                LoanPeriod = loan.LoanPeriod,
                LoanType = loan.LoanType,
                LoanCurrency = loan.LoanCurrency
            };

            loansDto.Loans.Add(loanDto);
        }

        return loansDto;
    }

    private async Task<ApplicationUser> GetUserFromHttpContext()
    {
        if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return null;
        }

        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        if (user == null)
        {
            return null;
        }

        return user;
    }
}