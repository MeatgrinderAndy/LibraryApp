using LibraryApp.Domain.Entities;

namespace LibraryApp.Infrastructure.Repository.Interface
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetByIsbnAsync(string isbn);
        Task<IEnumerable<Book>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
