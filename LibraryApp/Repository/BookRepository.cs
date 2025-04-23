using LibraryApp.Data;
using LibraryApp.Models;
using LibraryApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
            => await _context.Books
                .Include(b => b.Author)
                .ToListAsync();

        public async Task<Book?> GetByIdAsync(int id)
            => await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);

        public async Task<Book?> GetByIsbnAsync(string isbn)
            => await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.ISBN == isbn);

        public async Task AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorId(int authorId)
            => await _context.Books.Where(b => b.Author.Id == authorId).ToListAsync();
    }
}
