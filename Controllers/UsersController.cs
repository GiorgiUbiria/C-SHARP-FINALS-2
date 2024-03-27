using Asp.Versioning;
using Finals.Dtos;
using Finals.Models;
using Finals.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finals.Controllers;

[ApiVersion(1.0)]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;
    private readonly IValidator<RegisterRequestDto> _registerValidator;
    private readonly IValidator<AuthRequestDto> _authValidator;

    public UsersController(IUserService userService, ILogger<UsersController> logger,
        IValidator<AuthRequestDto> authValidator, IValidator<RegisterRequestDto> registerValidator)
    {
        _userService = userService;
        _logger = logger;
        _registerValidator = registerValidator;
        _authValidator = authValidator;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        _logger.LogInformation("Attempting to register user with email: {Email}", request.Email);

        var validationResult = await _registerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogInformation("Invalid model state.");
            return BadRequest(validationResult.Errors);
        }

        var result = await _userService.RegisterUser(request);

        if (result.User.Succeeded)
        {
            request.Password = "";
            _logger.LogInformation("User registered successfully.");
            return CreatedAtAction(nameof(Register), new { email = request.Email }, request);
        }

        foreach (var error in result.User.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        _logger.LogInformation("Failed to register user.");
        return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponseDto>> Authenticate([FromBody] AuthRequestDto request)
    {
        _logger.LogInformation("Attempting user authentication.");

        var validationResult = await _authValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogInformation("Invalid model state.");
            return BadRequest(validationResult.Errors);
        }

        var authResponse = await _userService.AuthenticateUser(request);
        if (authResponse == null)
        {
            _logger.LogInformation("Authentication failed: Bad credentials.");
            return BadRequest("Bad credentials");
        }

        _logger.LogInformation("User authenticated successfully.");
        return Ok(authResponse);
    }

    [HttpGet]
    [Authorize]
    [Route("user/me")]
    public async Task<ActionResult<ApplicationUser>> GetCurrentUser()
    {
        _logger.LogInformation("Attempting to retrieve current user.");

        var user = await _userService.GetCurrentUser(User);
        if (user == null)
        {
            _logger.LogInformation("Current user not found.");
            return NotFound();
        }

        _logger.LogInformation("Current user retrieved successfully.");
        return Ok(user);
    }

    [HttpGet]
    [Authorize]
    [Route("user")]
    public async Task<ActionResult<ApplicationUser>> GetUserByEmail([FromQuery] string email)
    {
        _logger.LogInformation("Attempting to retrieve user by email: {Email}", email);

        try
        {
            var user = await _userService.GetUserByEmail(email, User);
            if (user == null)
            {
                _logger.LogInformation("User with email {Email} not found.", email);
                return NotFound();
            }

            _logger.LogInformation("User with email {Email} retrieved successfully.", email);
            return Ok(user);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogInformation("Unauthorized access: User not authenticated.");
            return Forbid();
        }
    }

    [HttpPost]
    [Authorize(Roles = "Accountant")]
    [Route("user/block")]
    public async Task<IActionResult> BlockUser(string email)
    {
        _logger.LogInformation("Attempting to block user with Email: {email}", email);

        try
        {
            var result = await _userService.BlockUser(email);
            if (result.Action)
            {
                _logger.LogInformation("User with Email {email} blocked successfully.", email);
                return Ok(result);
            }
            else
            {
                _logger.LogInformation("Failed to block user with Email {email}.", email);
                return StatusCode(500);
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error blocking user with Email {email}: {ErrorMessage}", email, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogInformation("Unauthorized access: User not authorized to block.");
            return Forbid();
        }
    }

    [HttpPost]
    [Authorize(Roles = "Accountant")]
    [Route("user/unblock")]
    public async Task<IActionResult> UnblockUser(string email)
    {
        _logger.LogInformation("Attempting to unblock user with Email: {email}", email);

        try
        {
            var result = await _userService.UnblockUser(email);
            if (result.Action)
            {
                _logger.LogInformation("User with Email {email} unblocked successfully.", email);
                return Ok(result);
            }
            else
            {
                _logger.LogInformation("Failed to unblock user with Email {email}.", email);
                return StatusCode(500);
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error unblocking user with Email {email}: {ErrorMessage}", email, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogInformation("Unauthorized access: User not authorized to unblock.");
            return Forbid();
        }
    }

    [HttpPost]
    [Authorize(Roles = "Accountant")]
    [Route("user/make-accountant")]
    public async Task<IActionResult> MakeAccountant(string email)
    {
        _logger.LogInformation("Attempting to make user with Email: {email} an accountant.", email);

        try
        {
            var result = await _userService.MakeAccountant(email);
            if (result.Action)
            {
                _logger.LogInformation("User with Email {email} made accountant successfully.",email);
                return Ok(result);
            }
            else
            {
                _logger.LogInformation("Failed to make user with Email {email} an accountant.", email);
                return StatusCode(500);
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error making user with Email {email} an accountant: {ErrorMessage}", email,
                ex.Message);
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogInformation("Unauthorized access: User not authorized to make accountant.");
            return Forbid();
        }
    }
}