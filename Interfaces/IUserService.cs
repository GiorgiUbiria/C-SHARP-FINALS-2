using Finals.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Finals.Services;

public interface IUserService
{
    Task<IdentityResult> RegisterUser(RegisterRequestDto request);
    Task<AuthResponseDto> AuthenticateUser(LoginRequestDto request);
}