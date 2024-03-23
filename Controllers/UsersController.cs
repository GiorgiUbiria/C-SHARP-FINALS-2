using Finals.Dtos;
using Finals.Models;
using Finals.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finals.Controllers;

[ApiController]
[Route("/api/[controller]")]
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

        if (result.Succeeded)
        {
            request.Password = "";
            _logger.LogInformation("User registered successfully.");
            return CreatedAtAction(nameof(Register), new { email = request.Email }, request);
        }

        foreach (var error in result.Errors)
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
    [Route("me")]
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
    [Route("block")]
    [Authorize(Roles = "Accountant")]
    public async Task<IActionResult> BlockUser(string email)
    {
        _logger.LogInformation("Attempting to block user with Email: {email}", email);

        try
        {
            var result = await _userService.BlockUser(email);
            if (result)
            {
                _logger.LogInformation("User with Email {email} blocked successfully.", email);
                return Ok();
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
            return StatusCode(403);
        }
    }

    [HttpPost]
    [Route("unblock")]
    [Authorize(Roles = "Accountant")]
    public async Task<IActionResult> UnblockUser(string userId)
    {
        _logger.LogInformation("Attempting to unblock user with ID: {UserId}", userId);

        try
        {
            var result = await _userService.UnblockUser(userId);
            if (result)
            {
                _logger.LogInformation("User with ID {UserId} unblocked successfully.", userId);
                return Ok();
            }
            else
            {
                _logger.LogInformation("Failed to unblock user with ID {UserId}.", userId);
                return StatusCode(500);
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error unblocking user with ID {UserId}: {ErrorMessage}", userId, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogInformation("Unauthorized access: User not authorized to unblock.");
            return StatusCode(403);
        }
    }

    [HttpPost]
    [Route("make-accountant")]
    [Authorize(Roles = "Accountant")]
    public async Task<IActionResult> MakeAccountant(string userId)
    {
        _logger.LogInformation("Attempting to make user with ID: {UserId} an accountant.", userId);

        try
        {
            var result = await _userService.MakeAccountant(userId);
            if (result)
            {
                _logger.LogInformation("User with ID {UserId} made accountant successfully.", userId);
                return Ok();
            }
            else
            {
                _logger.LogInformation("Failed to make user with ID {UserId} an accountant.", userId);
                return StatusCode(500);
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error making user with ID {UserId} an accountant: {ErrorMessage}", userId,
                ex.Message);
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogInformation("Unauthorized access: User not authorized to make accountant.");
            return StatusCode(403);
        }
    }
}