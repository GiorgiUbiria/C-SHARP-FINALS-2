using Asp.Versioning;
using Finals.Contexts;
using Finals.Dtos;
using Finals.Interfaces;
using Finals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finals.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [Authorize(Roles = "Accountant")]
    [HttpPost("new")]
    public async Task<ActionResult<Loan>> CreateLoan(LoanDto loanDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var loan = await _loanService.CreateLoan(loanDto);
        return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loan);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LoanDto>> GetLoan(int id)
    {
        var loanDto = await _loanService.GetLoan(id);
        if (loanDto == null)
        {
            return NotFound();
        }

        return loanDto;
    }

    [HttpGet]
    public async Task<LoanDtos> GetAllLoans()
    {
        return await _loanService.GetAllLoans();
    }
}