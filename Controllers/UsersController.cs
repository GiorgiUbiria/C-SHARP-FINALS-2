using Finals.Dtos;
using Finals.Models;
using Finals.Services;
using Microsoft.AspNetCore.Authorization;
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
    
    [HttpPost]
    [Route("block")]
    [Authorize(Roles = "Accountant")]
    public async Task<IActionResult> BlockUser(string userId)
    {
        try
        {
            var result = await _userService.BlockUser(userId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            return StatusCode(403);
        }
    }
    
    [HttpPost]
    [Route("unblock")]
    [Authorize(Roles = "Accountant")]
    public async Task<IActionResult> UnblockUser(string userId)
    {
        try
        {
            var result = await _userService.UnblockUser(userId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            return StatusCode(403);
        }
    }
    
    [HttpPost]
    [Route("make-accountant")]
    [Authorize(Roles = "Accountant")]
    public async Task<IActionResult> MakeAccountant(string userId)
    {
        try
        {
            var result = await _userService.MakeAccountant(userId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            return StatusCode(403);
        }
    }
}