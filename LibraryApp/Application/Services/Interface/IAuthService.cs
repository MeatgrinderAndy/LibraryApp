using LibraryApp.Application.DTO.Auth;

namespace LibraryApp.Application.Services.Interface
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto dto);
        Task RegisterAsync(RegisterDto dto);
    }
}
