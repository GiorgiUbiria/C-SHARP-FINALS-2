using Asp.Versioning;
using Finals.Dtos;
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
    private readonly IInstallmentLoanService _installmentLoanService;
    private readonly ILogger<LoansController> _logger;
    private readonly IValidator<LoanRequestDto> _fastLoanValidator;
    private readonly IValidator<InstallmentLoanRequestDto> _installmentLoanValidator;

    public LoansController(ILoanService loanService, IInstallmentLoanService installmentLoanService, ILogger<LoansController> logger,
        IValidator<LoanRequestDto> fastLoanValidator, IValidator<InstallmentLoanRequestDto> installmentLoanValidator)
    {
        _loanService = loanService;
        _installmentLoanService = installmentLoanService;
        _logger = logger;
        _fastLoanValidator = fastLoanValidator;
        _installmentLoanValidator = installmentLoanValidator;
    }

    [HttpPost("new-fast-loan")]
    [Authorize]
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
            var loan = await _loanService.CreateLoan(loanDto);
            _logger.LogInformation("Fast loan created successfully.");
            return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loan);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error creating a fast loan: {ErrorMessage}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    
    [HttpPost("new-installment-loan")]
    [Authorize]
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
            return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loan);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error creating an installment loan: {ErrorMessage}", ex.Message);
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<LoanDto>> GetLoan(int id)
    {
        _logger.LogInformation("Attempting to retrieve loan with ID: {LoanId}", id);

        var loanDto = await _loanService.GetLoan(id);
        if (loanDto == null)
        {
            _logger.LogInformation("Loan with ID {LoanId} not found.", id);
            return NotFound();
        }

        _logger.LogInformation("Loan with ID {LoanId} retrieved successfully.", id);
        return loanDto;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllLoans()
    {
        _logger.LogInformation("Attempting to retrieve all loans.");

        var loanDtos = await _loanService.GetAllLoans();

        if (loanDtos == null || loanDtos.Loans.Count == 0)
        {
            _logger.LogInformation("No loans found.");
            return NoContent();
        }

        _logger.LogInformation("All loans retrieved successfully.");
        return Ok(loanDtos);
    }

    [HttpGet]
    [Authorize]
    [Route("pending")]
    public async Task<IActionResult> GetPendingLoans()
    {
        _logger.LogInformation($"Attempting to retrieve pending loans.");

        var loanDtos = await _loanService.GetPendingLoans();

        if (loanDtos == null || loanDtos.Loans.Count == 0)
        {
            _logger.LogInformation("No pending loans found.");
            return NoContent();
        }

        _logger.LogInformation($"Pending loans retrieved successfully.");
        return Ok(loanDtos);
    }

    [HttpGet]
    [Authorize]
    [Route("declined")]
    public async Task<IActionResult> GetDeclinedLoans()
    {
        _logger.LogInformation($"Attempting to retrieve declined loans.");

        var loanDtos = await _loanService.GetDeclinedLoans();

        if (loanDtos == null || loanDtos.Loans.Count == 0)
        {
            _logger.LogInformation("No declined loans found.");
            return NoContent();
        }

        _logger.LogInformation($"Declined loans retrieved successfully.");
        return Ok(loanDtos);
    }

    [HttpGet]
    [Authorize]
    [Route("accepted")]
    public async Task<IActionResult> GetAccpetedLoans()
    {
        _logger.LogInformation($"Attempting to retrieve accepted loans.");

        var loanDtos = await _loanService.GetAcceptedLoans();

        if (loanDtos == null || loanDtos.Loans.Count == 0)
        {
            _logger.LogInformation("No accepted loans found.");
            return NoContent();
        }

        _logger.LogInformation($"Accepted loans retrieved successfully.");
        return Ok(loanDtos);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Accountant")]
    public async Task<IActionResult> DeleteLoan(int id)
    {
        _logger.LogInformation("Attempting to delete loan with ID: {LoanId}", id);

        try
        {
            var result = await _loanService.DeleteLoan(id);
            if (result)
            {
                _logger.LogInformation("Loan with ID {LoanId} deleted successfully.", id);
                return Ok();
            }
            else
            {
                _logger.LogInformation("Loan with ID {LoanId} not found.", id);
                return NotFound();
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error deleting loan with ID {LoanId}: {ErrorMessage}", id, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Customer, Accountant")]
    public async Task<IActionResult> ModifyLoan(int id, [FromBody] LoanRequestDto loanDto)
    {
        _logger.LogInformation("Attempting to modify loan with ID: {LoanId}", id);

        var validationResult = await _fastLoanValidator.ValidateAsync(loanDto);

        if (!validationResult.IsValid)
        {
            _logger.LogInformation("Form Data is Invalid.");
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var modifiedLoanDto = await _loanService.ModifyLoan(id, loanDto);
            _logger.LogInformation("Loan with ID {LoanId} modified successfully.", id);
            return Ok(modifiedLoanDto);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error modifying loan with ID {LoanId}: {ErrorMessage}", id, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/accept")]
    [Authorize(Roles = "Accountant")]
    public async Task<IActionResult> AcceptLoan(int id)
    {
        _logger.LogInformation("Attempting to accept loan with ID: {LoanId}", id);

        try
        {
            var result = await _loanService.AcceptLoan(id);
            if (result)
            {
                _logger.LogInformation("Loan with ID {LoanId} accepted successfully.", id);
                return Ok();
            }
            else
            {
                _logger.LogInformation("Loan with ID {LoanId} not found.", id);
                return NotFound();
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error accepting loan with ID {LoanId}: {ErrorMessage}", id, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/decline")]
    [Authorize(Roles = "Accountant")]
    public async Task<IActionResult> DeclineLoan(int id)
    {
        _logger.LogInformation("Attempting to decline loan with ID: {LoanId}", id);

        try
        {
            var result = await _loanService.DeclineLoan(id);
            if (result)
            {
                _logger.LogInformation("Loan with ID {LoanId} declined successfully.", id);
                return Ok();
            }
            else
            {
                _logger.LogInformation("Loan with ID {LoanId} not found.", id);
                return NotFound();
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error declining loan with ID {LoanId}: {ErrorMessage}", id, ex.Message);
            return BadRequest(ex.Message);
        }
    }
}