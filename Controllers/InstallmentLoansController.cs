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
public class InstallmentLoansController : ControllerBase
{
    private readonly IInstallmentLoanService _installmentLoanService;
    private readonly ILogger<LoansController> _logger;
    private readonly IValidator<InstallmentLoanRequestDto> _installmentLoanValidator;

    public InstallmentLoansController(IInstallmentLoanService installmentLoanService,
        ILogger<LoansController> logger,
        IValidator<InstallmentLoanRequestDto> installmentLoanValidator)
    {
        _installmentLoanService = installmentLoanService;
        _logger = logger;
        _installmentLoanValidator = installmentLoanValidator;
    }

    [HttpPost]
    [Authorize]
    [Route("new-installment")]
    public async Task<ActionResult<Loan>> CreateInstallmentLoan([FromBody] InstallmentLoanRequestDto loanDto)
    {
        _logger.LogInformation("Attempting to create a new installment loan.");

        var validationResult = await _installmentLoanValidator.ValidateAsync(loanDto);

        if (!validationResult.IsValid)
        {
            _logger.LogInformation("Form Data is Invalid.");
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var loan = await _installmentLoanService.CreateInstallmentLoan(loanDto);
            _logger.LogInformation("Installment loan created successfully.");

            return Ok(loan);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error creating an installment loan: {ErrorMessage}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> ModifyLoan(int id, [FromBody] InstallmentLoanRequestDto installmentLoanRequestDto)
    {
        _logger.LogInformation("Attempting to modify loan with ID: {LoanId}", id);

        var validationResult = await _installmentLoanValidator.ValidateAsync(installmentLoanRequestDto);

        if (!validationResult.IsValid)
        {
            _logger.LogInformation("Form Data is Invalid.");
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var modifiedLoanDto = await _installmentLoanService.ModifyInstallmentLoan(id, installmentLoanRequestDto);
            _logger.LogInformation("Loan with ID {LoanId} modified successfully.", id);
            return Ok(modifiedLoanDto);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error modifying loan with ID {LoanId}: {ErrorMessage}", id, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [NonAction]
    public async Task<ActionResult<InstallmentLoanDto>> GetInstallmentLoan(int id)
    {
        _logger.LogInformation("Attempting to retrieve loan with ID: {LoanId}", id);

        var loanDto = await _installmentLoanService.GetLoan(id);
        if (loanDto == null)
        {
            _logger.LogInformation("Loan with ID {LoanId} not found.", id);
            return NotFound();
        }

        _logger.LogInformation("Loan with ID {LoanId} retrieved successfully.", id);
        return loanDto;
    }
}