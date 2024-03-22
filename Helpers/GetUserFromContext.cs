using System.Security.Claims;
using Finals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Finals.Helpers;

public class GetUserFromContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GetUserFromContext> _logger;

    public GetUserFromContext(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, ILogger<GetUserFromContext> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _logger = logger;
    } 
    
    public async Task<ApplicationUser> GetUser()
    {
        _logger.LogInformation("GetUser method is being called.");
        var claims = _httpContextAccessor.HttpContext?.User.Claims;
        if (claims != null)
        {
            var userIdClaims = claims.Where(c => c.Type == ClaimTypes.NameIdentifier).ToList();
            if (userIdClaims.Count > 1)
            {
                var userIdClaim = userIdClaims[1].Value;
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userIdClaim);
                if (user != null)
                {
                    _logger.LogInformation("Found the current User.");
                    return user;
                }
            }
        }

        _logger.LogWarning("User ID claim not found or user not found.");
        return null;
    }
}