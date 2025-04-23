using LibraryApp.DTO;
using LibraryApp.Models;

namespace LibraryApp.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(int id);
        Task<Author> CreateAsync(AuthorDto dto);
        Task<Author?> UpdateAsync(int id, AuthorDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId);

    }

}
