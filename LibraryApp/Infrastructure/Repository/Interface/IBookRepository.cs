using LibraryApp.Models;

namespace LibraryApp.Infrastructure.Repository.Interface
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetByIsbnAsync(string isbn);

    }
}
