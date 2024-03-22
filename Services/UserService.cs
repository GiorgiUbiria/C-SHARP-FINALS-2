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
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
        ITokenService tokenService, GetUserFromContext getUserFromContext, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
        _getUserFromContext = getUserFromContext;
        _logger = logger;
    }

    public async Task<IdentityResult> RegisterUser(RegisterRequestDto request)
    {
        try
        {
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                Role = Role.Customer,
                LastName = request.LastName,
                FirstName = request.FirstName,
                IsBlocked = false,
                Salary = request.Salary,
                Age = request.Age
            };

            var result = await _userManager.CreateAsync(user, request.Password!);
            _logger.LogInformation("User created successfully");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while registering user: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<AuthResponseDto> AuthenticateUser(LoginRequestDto request)
    {
        try
        {
            _logger.LogInformation("Authenticating user with email: {Email}", request.Email);

            var managedUser = await _userManager.FindByEmailAsync(request.Email!);
            if (managedUser == null)
            {
                _logger.LogInformation("User with email {Email} not found.", request.Email);
                return null;
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password!);
            if (!isPasswordValid)
            {
                _logger.LogInformation("Invalid password for user with email: {Email}", request.Email);
                return null;
            }

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (userInDb == null)
            {
                _logger.LogInformation("User with email {Email} not found in the database.", request.Email);
                return null;
            }

            var accessToken = _tokenService.CreateToken(userInDb);

            _logger.LogInformation("User authenticated successfully with email: {Email}", request.Email);

            return new AuthResponseDto
            {
                Email = userInDb.Email!,
                Token = accessToken,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while authenticating user with email: {Email}. Error: {ErrorMessage}",
                request.Email, ex.Message);
            throw;
        }
    }

    public async Task<ApplicationUser> GetCurrentUser(ClaimsPrincipal userClaims)
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve current user.");

            var currentUser = await _getUserFromContext.GetUser();
            return currentUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving current user: {ErrorMessage}", ex.Message);
            return null;
        }
    }

    public async Task<ApplicationUser> GetUserByEmail(string email, ClaimsPrincipal userClaims)
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve user by email: {Email}", email);

            var currentUser = await GetCurrentUser(userClaims);

            if (currentUser.Role != Role.Accountant)
            {
                _logger.LogInformation("Unauthorized access: Current user does not have sufficient privileges.");
                throw new UnauthorizedAccessException(
                    "Only users with the Accountant role can access other users' details.");
            }

            var user = await _userManager.FindByEmailAsync(email);

            _logger.LogInformation("User with email {Email} retrieved successfully.", email);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving user by email: {Email}. Error: {ErrorMessage}", email,
                ex.Message);
            throw;
        }
    }

    public async Task<bool> BlockUser(string userId)
    {
        try
        {
            _logger.LogInformation("Attempting to block user with ID: {UserId}", userId);

            var currentUser = await _getUserFromContext.GetUser();
            if (currentUser == null)
            {
                _logger.LogInformation("Current user not found.");
                throw new InvalidOperationException("Current user not found.");
            }

            if (currentUser.Role != Role.Accountant)
            {
                _logger.LogInformation("Unauthorized access: Current user does not have sufficient privileges.");
                throw new UnauthorizedAccessException("Only users with the Accountant role can block other users.");
            }

            var userToBlock = await _userManager.FindByIdAsync(userId);
            if (userToBlock == null)
            {
                _logger.LogInformation("User to block not found.");
                throw new InvalidOperationException("User to block not found.");
            }

            if (userToBlock.Role != Role.Customer)
            {
                _logger.LogInformation("User is not a customer.");
                throw new InvalidOperationException("Only users with the Customer role can be blocked.");
            }

            if (userToBlock.IsBlocked)
            {
                _logger.LogInformation("User is already blocked.");
                throw new InvalidOperationException("User is already blocked.");
            }

            userToBlock.IsBlocked = true;
            var result = await _userManager.UpdateAsync(userToBlock);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} blocked successfully.", userId);
                return true;
            }
            else
            {
                _logger.LogError("Failed to block user with ID {UserId}: {Errors}", userId,
                    string.Join(", ", result.Errors));
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while blocking user with ID: {UserId}. Error: {ErrorMessage}", userId,
                ex.Message);
            throw;
        }
    }

    public async Task<bool> UnblockUser(string userId)
    {
        try
        {
            _logger.LogInformation("Attempting to unblock user with ID: {UserId}", userId);

            var currentUser = await _getUserFromContext.GetUser();
            if (currentUser == null)
            {
                _logger.LogInformation("Current user not found.");
                throw new InvalidOperationException("Current user not found.");
            }

            if (currentUser.Role != Role.Accountant)
            {
                _logger.LogInformation("Unauthorized access: Current user does not have sufficient privileges.");
                throw new UnauthorizedAccessException("Only users with the Accountant role can unblock other users.");
            }

            var userToUnblock = await _userManager.FindByIdAsync(userId);
            if (userToUnblock == null)
            {
                _logger.LogInformation("User to unblock not found.");
                throw new InvalidOperationException("User to unblock not found.");
            }

            if (userToUnblock.Role != Role.Customer)
            {
                _logger.LogInformation("User is not a customer.");
                throw new InvalidOperationException("Only users with the Customer role can be unblocked.");
            }

            if (!userToUnblock.IsBlocked)
            {
                _logger.LogInformation("User is not blocked.");
                throw new InvalidOperationException("User is not blocked.");
            }

            userToUnblock.IsBlocked = false;
            var result = await _userManager.UpdateAsync(userToUnblock);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} unblocked successfully.", userId);
                return true;
            }
            else
            {
                _logger.LogError("Failed to unblock user with ID {UserId}: {Errors}", userId,
                    string.Join(", ", result.Errors));
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while unblocking user with ID: {UserId}. Error: {ErrorMessage}",
                userId, ex.Message);
            throw;
        }
    }

    public async Task<bool> MakeAccountant(string userId)
    {
        try
        {
            _logger.LogInformation("Attempting to make user with ID: {UserId} an accountant.", userId);

            var currentUser = await _getUserFromContext.GetUser();
            if (currentUser == null)
            {
                _logger.LogInformation("Current user not found.");
                throw new InvalidOperationException("Current user not found.");
            }

            if (currentUser.Role != Role.Accountant)
            {
                _logger.LogInformation("Unauthorized access: Current user does not have sufficient privileges.");
                throw new UnauthorizedAccessException(
                    "Only users with the Accountant role can upgrade other users to Accountant.");
            }

            var userToModify = await _userManager.FindByIdAsync(userId);
            if (userToModify == null)
            {
                _logger.LogInformation("User to modify not found.");
                throw new InvalidOperationException("User to modify not found.");
            }

            if (userToModify.Role != Role.Customer)
            {
                _logger.LogInformation("User is not a customer.");
                throw new InvalidOperationException("Only users with the Customer role can be upgraded to Accountant.");
            }

            if (userToModify.IsBlocked)
            {
                _logger.LogInformation("User is blocked.");
                throw new InvalidOperationException("User is blocked.");
            }

            userToModify.Role = Role.Accountant;
            var result = await _userManager.UpdateAsync(userToModify);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} upgraded to Accountant successfully.", userId);
                return true;
            }
            else
            {
                _logger.LogError("Failed to upgrade user with ID {UserId} to Accountant: {Errors}", userId,
                    string.Join(", ", result.Errors));
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error occurred while making user with ID: {UserId} an accountant. Error: {ErrorMessage}", userId,
                ex.Message);
            throw;
        }
    }
}