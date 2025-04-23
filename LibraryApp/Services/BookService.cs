using LibraryApp.Models;
using LibraryApp.Repository.Interface;
using LibraryApp.Services.Interfaces;

namespace LibraryApp.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;

        public BookService(IBookRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Book>> GetBooksAsync() => _repo.GetAllAsync();
      

        public Task<Book?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task AddAsync(Book book) => _repo.AddAsync(book);

        public Task<Book?> GetByIsbnAsync(string isbn) => _repo.GetByIsbnAsync(isbn);
        

        public Task UpdateAsync(Book book) => _repo.UpdateAsync(book);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<IEnumerable<Book>> GetBooksByAuthorId(int authorId) => _repo.GetBooksByAuthorId(authorId);

    }
}
