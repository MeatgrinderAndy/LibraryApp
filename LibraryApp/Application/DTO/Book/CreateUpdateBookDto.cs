using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Application.DTO.Book
{
    public class CreateUpdateBookDto
    {
        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, ErrorMessage = "Название слишком длинное (максимум 100 символов)")]
        public string Title { get; set; }

        [Required(ErrorMessage = "ISBN обязателен")]
        [StringLength(15, MinimumLength = 13, ErrorMessage = "ISBN должен содержать от 13 до 15 символов")]

        public string ISBN { get; set; }

        [Required(ErrorMessage = "Жанр обязателен")]
        [StringLength(50, ErrorMessage = "Жанр слишком длинный")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Описание обязательно")]
        [StringLength(500, ErrorMessage = "Описание слишком длинное")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ID автора обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "ID автора должен быть положительным числом")]
        public int AuthorId { get; set; }
    }
}
