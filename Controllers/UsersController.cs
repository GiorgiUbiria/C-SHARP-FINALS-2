using Finals.Dtos;
using Finals.Models;
using Finals.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finals.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.RegisterUser(request);

        if (result.Succeeded)
        {
            request.Password = "";
            return CreatedAtAction(nameof(Register), new { email = request.Email }, request);
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponseDto>> Authenticate([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var authResponse = await _userService.AuthenticateUser(request);
        if (authResponse == null)
        {
            return BadRequest("Bad credentials");
        }

        return Ok(authResponse);
    }

    [HttpGet]
    [Route("me")]
    public async Task<ActionResult<ApplicationUser>> GetCurrentUser()
    {
        var user = await _userService.GetCurrentUser(User);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet]
    [Route("user")]
    public async Task<ActionResult<ApplicationUser>> GetUserByEmail([FromQuery] string email)
    {
        try
        {
            var user = await _userService.GetUserByEmail(email, User);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}