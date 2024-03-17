using Asp.Versioning;
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

    [Authorize]
    [HttpPost("new_loan")]
    public async Task<ActionResult<Loan>> CreateLoan(LoanDto loanDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var loan = await _loanService.CreateLoan(loanDto);
            return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loan);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
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
    public async Task<IActionResult> GetAllLoans()
    {
        var loanDtos = await _loanService.GetAllLoans();

        if (loanDtos == null || loanDtos.Loans.Count == 0)
        {
            return NoContent();
        }

        return Ok(loanDtos);
    }
}