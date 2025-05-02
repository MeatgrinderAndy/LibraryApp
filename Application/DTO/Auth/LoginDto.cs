using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Application.DTO.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Пароль должен быть минимум 5 символов")]
        public string Password { get; set; } = null!;
    }

}
