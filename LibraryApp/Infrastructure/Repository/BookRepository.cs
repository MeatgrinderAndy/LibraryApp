using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Data;
using LibraryApp.Infrastructure.Repository.Interface;
using LibraryApp.Infrastructure.Repository.LibraryApp.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Infrastructure.Repository
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base(context) { }

        public async Task<Book?> GetByIsbnAsync(string isbn)
        {
            return await _context.Books.Include(b => b.Author)
                                       .FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorId(int authorId)
        {
            return await _context.Books.Where(b => b.AuthorId == authorId).ToListAsync();
        }

        public virtual async Task<IEnumerable<Book>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var items = await _context.Books
                .OrderBy(b => b.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return items;
        }
    }
}

