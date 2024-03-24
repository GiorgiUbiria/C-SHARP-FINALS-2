using System.Security.Claims;
using Finals.Dtos;
using Finals.Models;
using Microsoft.AspNetCore.Identity;

namespace Finals.Services;

public interface IUserService
{
    Task<RegisterResponseDto> RegisterUser(RegisterRequestDto request);
    Task<AuthResponseDto> AuthenticateUser(AuthRequestDto request);
    Task<ApplicationUser> GetCurrentUser(ClaimsPrincipal userClaims);
    Task<ApplicationUser> GetUserByEmail(string email, ClaimsPrincipal userClaims);
    Task<bool> BlockUser(string email);
    Task<bool> UnblockUser(string email);
    Task<bool> MakeAccountant(string email);
}