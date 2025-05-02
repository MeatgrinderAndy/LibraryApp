using LibraryApp.Application.DTO.Author;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Services.Interface
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
