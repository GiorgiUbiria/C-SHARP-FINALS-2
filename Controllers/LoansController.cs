using Asp.Versioning;
using Finals.Dtos;
using Finals.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finals.Controllers;

[ApiVersion(1.0)]
[Route("api/[controller]")]
[ApiController]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;
    private readonly ILogger<LoansController> _logger;

    public LoansController(ILoanService loanService, ILogger<LoansController> logger)
    {
        _loanService = loanService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllLoans()
    {
        _logger.LogInformation("Attempting to retrieve all loans.");

        var loansDto = await _loanService.GetAllLoans();

        if (loansDto == null || loansDto.Loans.Count == 0)
        {
            _logger.LogInformation("No loans found.");
            return NoContent();
        }

        _logger.LogInformation("All loans retrieved successfully.");
        return Ok(loansDto);
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

    [HttpDelete("{id}")]
    [Authorize]
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

        var loansDto = await _loanService.GetDeclinedLoans();

        if (loansDto == null || loansDto.Loans.Count == 0)
        {
            _logger.LogInformation("No declined loans found.");
            return NoContent();
        }

        _logger.LogInformation($"Declined loans retrieved successfully.");
        return Ok(loansDto);
    }

    [HttpGet]
    [Authorize]
    [Route("accepted")]
    public async Task<IActionResult> GetAccpetedLoans()
    {
        _logger.LogInformation($"Attempting to retrieve accepted loans.");

        var loansDto = await _loanService.GetAcceptedLoans();

        if (loansDto == null || loansDto.Loans.Count == 0)
        {
            _logger.LogInformation("No accepted loans found.");
            return NoContent();
        }

        _logger.LogInformation($"Accepted loans retrieved successfully.");
        return Ok(loansDto);
    }
}