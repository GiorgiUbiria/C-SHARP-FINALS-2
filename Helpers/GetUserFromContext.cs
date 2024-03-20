using System.Security.Claims;
using Finals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Finals.Helpers;

public class GetUserFromContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public async Task<ApplicationUser> GetUser()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier && c.Value == "80c8b6b1-e2b6-45e8-b044-8f2178a90111")?.Value;        
        
        var claims = _httpContextAccessor.HttpContext?.User.Claims;

        if (userIdClaim == null)
        {
            return null;
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userIdClaim);

        if (user == null)
        {
            return null;
        }

        return user;
    }
}