using Asp.Versioning;
using Finals.Dtos;
using Finals.Interfaces;
using Finals.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finals.Controllers;

[ApiVersion(1.0)]
[Route("api/[controller]")]
[ApiController]
public class FastLoansController : ControllerBase
{
    private readonly IFastLoanService _fastLoanService;
    private readonly ILogger<LoansController> _logger;
    private readonly IValidator<LoanRequestDto> _fastLoanValidator;

    public FastLoansController(IFastLoanService fastLoanService, ILogger<LoansController> logger,
        IValidator<LoanRequestDto> fastLoanValidator)
    {
        _fastLoanService = fastLoanService;
        _logger = logger;
        _fastLoanValidator = fastLoanValidator;
    }

    [HttpPost]
    [Authorize]
    [Route("new-fast")]
    public async Task<ActionResult<Loan>> CreateFastLoan([FromBody] LoanRequestDto loanDto)
    {
        _logger.LogInformation("Attempting to create a new fast loan.");

        var validationResult = await _fastLoanValidator.ValidateAsync(loanDto);

        if (!validationResult.IsValid)
        {
            _logger.LogInformation("Form Data is Invalid.");
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var loan = await _fastLoanService.CreateFastLoan(loanDto);
            _logger.LogInformation("Fast loan created successfully.");

            return Ok(loan);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error creating a fast loan: {ErrorMessage}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [NonAction]
    public async Task<ActionResult<LoanDto>> GetFastLoan(int id)
    {
        _logger.LogInformation("Attempting to retrieve loan with ID: {LoanId}", id);

        var loanDto = await _fastLoanService.GetFastLoan(id);
        if (loanDto == null)
        {
            _logger.LogInformation("Loan with ID {LoanId} not found.", id);
            return NotFound();
        }

        _logger.LogInformation("Loan with ID {LoanId} retrieved successfully.", id);
        return loanDto;
    }
}