using LibraryApp.Data;
using LibraryApp.Models;
using LibraryApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
            => await _context.Authors.ToListAsync();

        public async Task<Author?> GetByIdAsync(int id)
            => await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

        public async Task AddAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId)
            => await _context.Books
                             .Where(b => b.AuthorId == authorId)
                             .ToListAsync();
    }

}
