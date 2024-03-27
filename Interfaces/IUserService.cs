using System.Security.Claims;
using Finals.Dtos;
using Finals.Models;
using Microsoft.AspNetCore.Identity;

namespace Finals.Services;

public interface IUserService
{
    Task<RegisterResponseDto> RegisterUser(RegisterRequestDto request);
    Task<AuthResponseDto> AuthenticateUser(AuthRequestDto request);
    Task<UserDto> GetCurrentUser(ClaimsPrincipal userClaims);
    Task<UserDto> GetUserByEmail(string email, ClaimsPrincipal userClaims);
    Task<UserStatusDto> BlockUser(string email);
    Task<UserStatusDto> UnblockUser(string email);
    Task<UserStatusDto> MakeAccountant(string email);
}