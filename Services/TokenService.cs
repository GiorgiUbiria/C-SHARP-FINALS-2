using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Finals.Interfaces;
using Finals.Models;
using Microsoft.IdentityModel.Tokens;

namespace Finals.Services;

public class TokenService : ITokenService
{
    private const int ExpirationMinutes = 30;
    private readonly ILogger<TokenService> _logger;

    public TokenService(ILogger<TokenService> logger)
    {
        _logger = logger;
    }

    public string CreateToken(ApplicationUser user)
    {
        try
        {
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
            var token = CreateJwtToken(
                CreateClaims(user),
                CreateSigningCredentials(),
                expiration
            );
            var tokenHandler = new JwtSecurityTokenHandler();

            _logger.LogInformation("JWT Token created successfully");

            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating JWT Token: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration) =>
        new JwtSecurityToken(
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")[
                "ValidIssuer"],
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")[
                "ValidAudience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private List<Claim> CreateClaims(ApplicationUser user)
    {
        try
        {
            var jwtSub =
                new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")[
                    "JwtRegisteredClaimNamesSub"];
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwtSub),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            return claims;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating claims: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    private SigningCredentials CreateSigningCredentials()
    {
        try
        {
            var symmetricSecurityKey =
                new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")[
                    "SymmetricSecurityKey"];
            return new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey)),
                SecurityAlgorithms.HmacSha256
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating signing credentials: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}