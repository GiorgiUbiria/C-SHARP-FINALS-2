using System.Security.Claims;
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
    private readonly ILogger<LoanService> _logger;

    public LoanService(ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ILogger<LoanService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }
    
    public async Task<bool> DeleteLoan(int id)
    {
        var user = await GetUser();
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id);
        if (loan == null)
        {
            throw new InvalidOperationException("Loan not found.");
        }

        if (user.Role == Role.Accountant)
        {
            _dbContext.Loans.Remove(loan);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        else if (user.Role == Role.Customer && loan.LoanStatus != LoanStatus.PENDING)
        {
            _dbContext.Loans.Remove(loan);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<LoanDto> ModifyLoan(int id, LoanDto loanDto)
    {
        var user = await GetUser();
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id);
        if (loan == null)
        {
            throw new InvalidOperationException("Loan not found.");
        }

        if (user.Role == Role.Accountant)
        {
            loan.Amount = loanDto.Amount;
            loan.LoanPeriod = loanDto.LoanPeriod;
            loan.LoanCurrency = loanDto.LoanCurrency;
            loan.LoanType = loanDto.LoanType;
            await _dbContext.SaveChangesAsync();
            return loanDto;
        }
        else if (user.Role == Role.Customer && loan.LoanStatus != LoanStatus.PENDING)
        {
            loan.Amount = loanDto.Amount;
            loan.LoanPeriod = loanDto.LoanPeriod;
            loan.LoanCurrency = loanDto.LoanCurrency;
            loan.LoanType = loanDto.LoanType;
            await _dbContext.SaveChangesAsync();
            return loanDto;
        }

        throw new InvalidOperationException("Operation not allowed.");
    }
    
    public async Task<Loan> CreateLoan(LoanDto loanDto)
    {
        var user = await GetUser();
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
        var user = await GetUser();
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
            LoanCurrency = loan.LoanCurrency,
            LoanStatus = loan.LoanStatus
        };

        return loanDto;
    }

    public async Task<LoanDtos> GetAllLoans()
    {
        var user = await GetUser();
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
                LoanCurrency = loan.LoanCurrency,
                LoanStatus = loan.LoanStatus
            };

            loansDto.Loans.Add(loanDto);
        }

        return loansDto;
    }

    public async Task<ApplicationUser> GetUser()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier && c.Value == "80c8b6b1-e2b6-45e8-b044-8f2178a90111")?.Value;        
        
        var claims = _httpContextAccessor.HttpContext?.User.Claims;

        if (userIdClaim == null)
        {
            return null;
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userIdClaim);

        if (user == null)
        {
            return null;
        }

        return user;
    }
}