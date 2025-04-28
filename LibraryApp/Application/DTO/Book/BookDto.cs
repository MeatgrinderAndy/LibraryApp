using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Application.DTO.Book
{
    public class BookDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название книги обязательно")]
        [StringLength(200, ErrorMessage = "Название не должно превышать 200 символов")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "ISBN обязателен")]
        [StringLength(15, MinimumLength = 13, ErrorMessage = "ISBN должен содержать от 13 до 15 символов")]
        public string ISBN { get; set; } = "";

        [Required(ErrorMessage = "Описание обязательно")]
        [StringLength(1000, ErrorMessage = "Описание не должно превышать 1000 символов")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Жанр обязателен")]
        [StringLength(100, ErrorMessage = "Жанр не должен превышать 100 символов")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Автор обязателен")]
        public int AuthorId { get; set; }
        public int? UserId { get; set; }

        public byte[]? CoverImage { get; set; }
        public string? AuthorName { get; set; }
        public DateOnly? DateWhenTaken { get; set; }
        public DateOnly? DateWhenNeedToReturn { get; set; }
    }

}
