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
    private readonly ApplicationDbContext _dbContext;
    private readonly ITokenService _tokenService;
    private readonly GetUserFromContext _getUserFromContext;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext,
        ITokenService tokenService, GetUserFromContext getUserFromContext, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _tokenService = tokenService;
        _getUserFromContext = getUserFromContext;
        _logger = logger;
    }

    public async Task<RegisterResponseDto> RegisterUser(RegisterRequestDto request)
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
            return new RegisterResponseDto
            {
                User = result,
                Message = "New user created successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while registering user: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<AuthResponseDto> AuthenticateUser(AuthRequestDto request)
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

            var userInDb = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
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

    public async Task<UserDto> GetCurrentUser(ClaimsPrincipal userClaims)
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve current user.");

            var currentUser = await _getUserFromContext.GetUser();

            var userDto = new UserDto
            {
                Role = currentUser.Role,
                IsBlocked = currentUser.IsBlocked,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Age = currentUser.Age,
                Salary = currentUser.Salary,
                Loans = currentUser.Loans,
            };

            return userDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving current user: {ErrorMessage}", ex.Message);
            return null;
        }
    }

    public async Task<UsersDto> GetAllUsers()
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve all users.");

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                return null;
            }

            IQueryable<ApplicationUser> usersFromDb = Enumerable.Empty<ApplicationUser>().AsQueryable();

            if (user.Role != Role.Accountant)
            {
                throw new InvalidOperationException("Only accountants can retrieve user information.");
            }

            usersFromDb = _dbContext.Users.AsQueryable();

            var usersDto = new UsersDto();

            if (usersFromDb == null || !usersFromDb.Any())
            {
                _logger.LogInformation("No users found.");
                return usersDto;
            }

            usersDto.Users = usersFromDb.Select(user => new UserDto
            {
                Role = user.Role,
                IsBlocked = user.IsBlocked,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                Salary = user.Salary,
                Loans = user.Loans,
            }).ToList();

            _logger.LogInformation("All users retrieved successfully.");
            return usersDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all loans: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<UserDto> GetUserByEmail(string email, ClaimsPrincipal userClaims)
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

            var userDto = new UserDto
            {
                Role = user.Role,
                IsBlocked = user.IsBlocked,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                Salary = user.Salary,
                Loans = user.Loans,
            };

            return userDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving user by email: {Email}. Error: {ErrorMessage}", email,
                ex.Message);
            throw;
        }
    }

    public async Task<UserStatusDto> BlockUser(string email)
    {
        try
        {
            _logger.LogInformation("Attempting to block user with Email: {email}", email);

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

            var userToBlock = await _userManager.FindByEmailAsync(email);
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
                _logger.LogInformation("User with Email {email} blocked successfully.", email);
                var userStatus = new UserStatusDto
                {
                    Email = userToBlock.Email,
                    isBlocked = userToBlock.IsBlocked,
                    Role = userToBlock.Role,
                    Action = true
                };

                return userStatus;
            }
            else
            {
                _logger.LogError("Failed to block user with Email {email}: {Errors}", email,
                    string.Join(", ", result.Errors));
                var userStatus = new UserStatusDto
                {
                    Email = userToBlock.Email,
                    isBlocked = userToBlock.IsBlocked,
                    Role = userToBlock.Role,
                    Action = false
                };

                return userStatus;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while blocking user with ID: {UserId}. Error: {ErrorMessage}", email,
                ex.Message);
            throw;
        }
    }

    public async Task<UserStatusDto> UnblockUser(string email)
    {
        try
        {
            _logger.LogInformation("Attempting to unblock user with Email: {email}", email);

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

            var userToUnblock = await _userManager.FindByEmailAsync(email);
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
                _logger.LogInformation("User with Email {email} unblocked successfully.", email);
                var userStatus = new UserStatusDto
                {
                    Email = userToUnblock.Email,
                    isBlocked = userToUnblock.IsBlocked,
                    Role = userToUnblock.Role,
                    Action = true
                };

                return userStatus;
            }
            else
            {
                _logger.LogError("Failed to unblock user with Email {email}: {Errors}", email,
                    string.Join(", ", result.Errors));
                var userStatus = new UserStatusDto
                {
                    Email = userToUnblock.Email,
                    isBlocked = userToUnblock.IsBlocked,
                    Role = userToUnblock.Role,
                    Action = false
                };

                return userStatus;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while unblocking user with Email: {email}. Error: {ErrorMessage}",
                email, ex.Message);
            throw;
        }
    }

    public async Task<UserStatusDto> MakeAccountant(string email)
    {
        try
        {
            _logger.LogInformation("Attempting to make user with Email: {email} an accountant.", email);

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

            var userToModify = await _userManager.FindByEmailAsync(email);
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
                _logger.LogInformation("User with Email {email} upgraded to Accountant successfully.", email);
                var userStatus = new UserStatusDto
                {
                    Email = userToModify.Email,
                    isBlocked = userToModify.IsBlocked,
                    Role = userToModify.Role,
                    Action = true
                };

                return userStatus;
            }
            else
            {
                _logger.LogError("Failed to upgrade user with Email {email} to Accountant: {Errors}", email,
                    string.Join(", ", result.Errors));
                var userStatus = new UserStatusDto
                {
                    Email = userToModify.Email,
                    isBlocked = userToModify.IsBlocked,
                    Role = userToModify.Role,
                    Action = false
                };

                return userStatus;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error occurred while making user with Email: {email} an accountant. Error: {ErrorMessage}", email,
                ex.Message);
            throw;
        }
    }
}