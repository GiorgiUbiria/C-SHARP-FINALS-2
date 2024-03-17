using Finals.Models;

namespace Finals.Interfaces;

public interface ITokenService
{
    string CreateToken(ApplicationUser user);
}