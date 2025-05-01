using System.Security.Claims;

namespace LibraryApp.Application.Services.Interface
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string username, string role);
        ClaimsPrincipal? ValidateToken(string token);
    }

}
