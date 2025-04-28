using LibraryApp.Application.DTO.Book;

namespace LibraryApp.Application.Services.Interface
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<BookDto?> GetByIdAsync(int id);
        Task<BookDto?> GetByIsbnAsync(string isbn);
        Task<BookDto> AddAsync(CreateUpdateBookDto dto, IFormFile? coverImage);
        Task UpdateAsync(int id, CreateUpdateBookDto dto, IFormFile? coverImage);
        Task DeleteAsync(int id);
        Task<IEnumerable<BookDto>> GetMyBooksAsync(int userId);
        Task ReturnBookAsync(int id);
        Task BorrowBookAsync(int id, int userId);
        Task UploadImageAsync(int id, IFormFile image);
        Task<byte[]?> GetImageAsync(int id);
        Task<IEnumerable<BookDto>> GetBooksPageAsync(int pageNumber, int rowsAmount);
    }
}
