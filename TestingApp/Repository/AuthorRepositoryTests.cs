using LibraryApp.Infrastructure.Data;
using LibraryApp.Infrastructure.Repository;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace TestingApp.Repository
{
    public class AuthorRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly AuthorRepository _repository;

        public AuthorRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _repository = new AuthorRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddAsync_ShouldAddAuthor()
        {
            var author = new Author { FirstName = "Test", LastName = "Author" , Country = "England", DateOfBirth = new DateOnly()};

            await _repository.AddAsync(author);

            var result = await _context.Authors.FirstOrDefaultAsync();
            Assert.NotNull(result);
            Assert.Equal("Test", result.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAuthor()
        {
            var author = new Author { Id = 1, FirstName = "Test", LastName = "Author", Country = "England", DateOfBirth = new DateOnly() };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }
    }
}