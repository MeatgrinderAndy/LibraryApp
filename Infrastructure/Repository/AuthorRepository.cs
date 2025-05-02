using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Data;
using LibraryApp.Infrastructure.Repository.Interface;
using LibraryApp.Infrastructure.Repository.LibraryApp.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Infrastructure.Repository
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId)
        {
            return await _context.Books
                .Where(b => b.AuthorId == authorId)
                .ToListAsync();
        }
    }
}
