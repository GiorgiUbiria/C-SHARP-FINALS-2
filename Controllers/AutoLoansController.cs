using Finals.Dtos;
using Finals.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finals.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoLoansController : ControllerBase
{
    private readonly IAutoLoanService _autoLoanService;
    private readonly ILogger<AutoLoansController> _logger;
    private readonly IValidator<AutoLoanRequestDto> _autoLoanValidator;

    public AutoLoansController(IAutoLoanService autoLoanService, ILogger<AutoLoansController> logger,
        IValidator<AutoLoanRequestDto> autoLoanValidator)
    {
        _autoLoanService = autoLoanService;
        _logger = logger;
        _autoLoanValidator = autoLoanValidator;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAutoLoan([FromBody] AutoLoanRequestDto autoLoanRequestDto)
    {
        _logger.LogInformation("Attempting to create a new auto loan.");

        var validationResult = await _autoLoanValidator.ValidateAsync(autoLoanRequestDto);

        if (!validationResult.IsValid)
        {
            _logger.LogInformation("Form Data is Invalid.");
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var autoLoan = await _autoLoanService.GetCarAndLoanAsync(autoLoanRequestDto);
            _logger.LogInformation("Auto loan created successfully.");
            return Ok(autoLoan.loan);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating an auto loan: {ErrorMessage}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> ModifyLoan(int id, [FromBody] AutoLoanRequestDto autoLoanRequestDto)
    {
        _logger.LogInformation("Attempting to modify loan with ID: {LoanId}", id);

        var validationResult = await _autoLoanValidator.ValidateAsync(autoLoanRequestDto);

        if (!validationResult.IsValid)
        {
            _logger.LogInformation("Form Data is Invalid.");
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var modifiedLoanDto = await _autoLoanService.ModifyAutoLoan(id, autoLoanRequestDto);
            _logger.LogInformation("Loan with ID {LoanId} modified successfully.", id);
            return Ok(modifiedLoanDto);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error modifying loan with ID {LoanId}: {ErrorMessage}", id, ex.Message);
            return BadRequest(ex.Message);
        }
    }
}