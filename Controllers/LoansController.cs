using Asp.Versioning;
using Finals.Interfaces;
using Finals.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finals.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;
    private readonly ILogger<LoansController> _logger;
    private readonly IValidator<LoanDto> _validator;

    public LoansController(ILoanService loanService, ILogger<LoansController> logger, IValidator<LoanDto> validator)
    {
        _loanService = loanService;
        _logger = logger;
        _validator = validator;
    }

    [Authorize]
    [HttpPost("new_loan")]
    public async Task<ActionResult<Loan>> CreateLoan([FromBody] LoanDto loanDto)
    {
        var validationResult = await _validator.ValidateAsync(loanDto);

        if (!validationResult.IsValid)
        {
            _logger.LogInformation("Form Data is Invalid.");
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var loan = await _loanService.CreateLoan(loanDto);
            _logger.LogInformation("Loan created successfully.");
            return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loan);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error creating loan: {ErrorMessage}", ex.Message);
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