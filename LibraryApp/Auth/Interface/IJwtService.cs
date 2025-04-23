using System.Security.Claims;

namespace LibraryApp.Auth.Interface
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string username, string role);
        ClaimsPrincipal? ValidateToken(string token);
    }

}
