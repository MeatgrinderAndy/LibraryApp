using LibraryApp.Application.DTO.Auth;
using LibraryApp.Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;


namespace LibraryApp.API.Controllers
{
    [Route("api/auth")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UsersController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            await _authService.RegisterAsync(dto);
            return Ok("Registration succesful");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);

            return Ok(new { token });
        }
    }
}
