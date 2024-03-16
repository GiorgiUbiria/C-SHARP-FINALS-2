using Asp.Versioning;
using Finals.Contexts;
using Finals.Dtos;
using Finals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finals.Controllers;

[ApiVersion( 1.0 )]
[ApiController]
[Route("api/[controller]" )]
public class LoansController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext; 
    
    public LoansController(ILogger<LoansController> logger, ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [Authorize (Roles = "Accountant")]
    [HttpPost("new")]
    public async Task<ActionResult<Loan>> CreatePage(LoanDto loanDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var loan = new Loan 
        {
            Id = loanDto.Id,
            Amount = loanDto.Amount ?? default(int),
            LoanPeriod = loanDto.LoanPeriod ?? default(DateTime)
        };
        
        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetPage), new { id = loan.Id }, loan);
    }
    
        
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LoanDto>> GetPage(int id)
    {
        var loan = await _dbContext.Loans.FindAsync(id);

        if (loan is null)
        {
            return NotFound();
        }
        
        var loanDto = new LoanDto
        {
            Id = loan.Id,
            Amount = loan.Amount,
            LoanPeriod = loan.LoanPeriod
        };

        return loanDto;
    }
    
    
    [HttpGet]
    public async Task<LoanDtos> ListPages()
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