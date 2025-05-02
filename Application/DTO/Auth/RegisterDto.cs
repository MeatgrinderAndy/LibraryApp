using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Application.DTO.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Имя пользователя должно быть от 3 до 30 символов")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Пароль должен быть минимум 5 символов")]
        public string Password { get; set; }

        public string Role { get; set; } = "User";
    }

}
