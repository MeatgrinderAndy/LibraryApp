using LibraryApp.Domain.Entities;

namespace LibraryApp.Infrastructure.Repository.Interface
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId);
    }
}
