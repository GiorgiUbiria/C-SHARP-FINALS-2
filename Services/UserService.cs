using System.Security.Claims;
using Finals.Contexts;
using Finals.Dtos;
using Finals.Enums;
using Finals.Helpers;
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
    private readonly GetUserFromContext _getUserFromContext;

    public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<IdentityResult> RegisterUser(RegisterRequestDto request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Username, Email = request.Email, Role = Role.Customer, LastName = request.LastName,
            FirstName = request.FirstName, IsBlocked = false, Salary = request.Salary, Age = request.Age
        };
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

    public async Task<ApplicationUser> GetCurrentUser(ClaimsPrincipal userClaims)
    {
        var userEmail = userClaims.FindFirstValue(ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(userEmail);
        return user;
    }

    public async Task<ApplicationUser> GetUserByEmail(string email, ClaimsPrincipal userClaims)
    {
        var currentUser = await GetCurrentUser(userClaims);
        if (currentUser.Role != Role.Accountant)
        {
            throw new UnauthorizedAccessException(
                "Only users with the Accountant role can access other users' details.");
        }

        var user = await _userManager.FindByEmailAsync(email);
        return user;
    }

    public async Task<bool> BlockUser(string userId)
    {
        var currentUser = await _getUserFromContext.GetUser();
        if (currentUser == null)
        {
            throw new InvalidOperationException("Current user not found.");
        }

        if (currentUser.Role != Role.Accountant)
        {
            throw new UnauthorizedAccessException("Only users with the Accountant role can block other users.");
        }

        var userToBlock = await _userManager.FindByIdAsync(userId);
        if (userToBlock == null)
        {
            throw new InvalidOperationException("User to block not found.");
        }

        if (userToBlock.Role != Role.Customer)
        {
            throw new InvalidOperationException("Only users with the Customer role can be blocked.");
        }

        if (userToBlock.IsBlocked)
        {
            throw new InvalidOperationException("User is already blocked.");
        }

        userToBlock.IsBlocked = true;
        var result = await _userManager.UpdateAsync(userToBlock);

        return result.Succeeded;
    }

    public async Task<bool> UnblockUser(string userId)
    {
        var currentUser = await _getUserFromContext.GetUser();
        if (currentUser == null)
        {
            throw new InvalidOperationException("Current user not found.");
        }

        if (currentUser.Role != Role.Accountant)
        {
            throw new UnauthorizedAccessException("Only users with the Accountant role can unblock other users.");
        }

        var userToUnblock = await _userManager.FindByIdAsync(userId);
        if (userToUnblock == null)
        {
            throw new InvalidOperationException("User to unblock not found.");
        }

        if (userToUnblock.Role != Role.Customer)
        {
            throw new InvalidOperationException("Only users with the Customer role can be unblocked.");
        }

        if (!userToUnblock.IsBlocked)
        {
            throw new InvalidOperationException("User is not blocked.");
        }

        userToUnblock.IsBlocked = false;
        var result = await _userManager.UpdateAsync(userToUnblock);

        return result.Succeeded;
    }
}