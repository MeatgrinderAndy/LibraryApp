using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Application.DTO.Author
{
    public class AuthorDto
    {
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(50, ErrorMessage = "Фамилия не должна превышать 50 символов")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна")]
        [DataType(DataType.Date, ErrorMessage = "Некорректная дата рождения")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Страна обязательна")]
        [StringLength(50, ErrorMessage = "Страна не должна превышать 50 символов")]
        public string Country { get; set; }
    }
}
