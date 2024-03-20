using System.Security.Claims;
using Finals.Dtos;
using Finals.Models;
using Microsoft.AspNetCore.Identity;

namespace Finals.Services;

public interface IUserService
{
    Task<IdentityResult> RegisterUser(RegisterRequestDto request);
    Task<AuthResponseDto> AuthenticateUser(LoginRequestDto request);
    Task<ApplicationUser> GetCurrentUser(ClaimsPrincipal userClaims);
    Task<ApplicationUser> GetUserByEmail(string email, ClaimsPrincipal userClaims);
}