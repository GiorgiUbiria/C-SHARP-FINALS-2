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

    [HttpDelete("{id}")]
    [Authorize(Roles = "Accountant")]
    public async Task<IActionResult> DeleteLoan(int id)
    {
        try
        {
            var result = await _loanService.DeleteLoan(id);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Customer, Accountant")]
    public async Task<IActionResult> ModifyLoan(int id, [FromBody] LoanDto loanDto)
    {
        try
        {
            var modifiedLoanDto = await _loanService.ModifyLoan(id, loanDto);
            return Ok(modifiedLoanDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/accept")]
    [Authorize(Roles = "Accountant")]
    public async Task<IActionResult> AcceptLoan(int id)
    {
        try
        {
            var result = await _loanService.AcceptLoan(id);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/decline")]
    [Authorize(Roles = "Accountant")]
    public async Task<IActionResult> DeclineLoan(int id)
    {
        try
        {
            var result = await _loanService.DeclineLoan(id);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}