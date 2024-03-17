using Finals.Contexts;
using Finals.Dtos;
using Finals.Enums;
using Finals.Interfaces;
using Finals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Finals.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ITokenService tokenService)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<IdentityResult> RegisterUser(RegisterRequestDto request)
    {
        var user = new ApplicationUser { UserName = request.Username, Email = request.Email, Role = Role.Customer };
        var result = await _userManager.CreateAsync(user, request.Password!);
        return result;
    }

    public async Task<AuthResponseDto> AuthenticateUser(LoginRequestDto request)
    {
        var managedUser = await _userManager.FindByEmailAsync(request.Email!);
        if (managedUser == null)
        {
            return null;
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password!);
        if (!isPasswordValid)
        {
            return null;
        }

        var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (userInDb == null)
        {
            return null;
        }

        var accessToken = _tokenService.CreateToken(userInDb);
        return new AuthResponseDto
        {
            Email = userInDb.Email,
            Token = accessToken,
        };
    } 
}