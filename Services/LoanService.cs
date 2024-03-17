using Finals.Contexts;
using Finals.Dtos;
using Finals.Interfaces;
using Finals.Models;
using Microsoft.EntityFrameworkCore;

namespace Finals.Services;

public class LoanService : ILoanService
{
    private readonly ApplicationDbContext _dbContext;

    public LoanService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Loan> CreateLoan(LoanDto loanDto)
    {
        var loan = new Loan
        {
            Amount = loanDto.Amount ?? default,
            LoanPeriod = loanDto.LoanPeriod ?? default
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();

        return loan;
    }

    public async Task<LoanDto> GetLoan(int id)
    {
        var loan = await _dbContext.Loans.FindAsync(id);
        if (loan == null)
        {
            return null;
        }

        var loanDto = new LoanDto
        {
            Id = loan.Id,
            Amount = loan.Amount,
            LoanPeriod = loan.LoanPeriod
        };

        return loanDto;
    }

    public async Task<LoanDtos> GetAllLoans()
    {
        var loansFromDb = await _dbContext.Loans.ToListAsync();
        var loansDto = new LoanDtos();

        foreach (var loan in loansFromDb)
        {
            var loanDto = new LoanDto
            {
                Id = loan.Id,
                Amount = loan.Amount,
                LoanPeriod = loan.LoanPeriod
            };

            loansDto.Loans.Add(loanDto);
        }

        return loansDto;
    } 
}