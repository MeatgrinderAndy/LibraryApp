using LibraryApp.DTO;
using LibraryApp.Models;
using LibraryApp.Repository.Interface;
using LibraryApp.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repository;

        public AuthorService(IAuthorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<Author?> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<Author> CreateAsync(AuthorDto authorDto)
        {
            if (!DateOnly.TryParse(authorDto.DateOfBirth, out var dateOfBirth))
            {
                throw new ValidationException("Некорректный формат даты. Используйте YYYY-MM-DD");
            }

            var author = new Author
            {
                FirstName = authorDto.FirstName,
                LastName = authorDto.LastName,
                DateOfBirth = dateOfBirth,
                Country = authorDto.Country
            };

            await _repository.AddAsync(author);
            return author;
        }

        public async Task<Author?> UpdateAsync(int id, AuthorDto authorDto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            if (!DateOnly.TryParse(authorDto.DateOfBirth, out var dateOfBirth))
            {
                throw new ValidationException("Некорректный формат даты. Используйте YYYY-MM-DD");
            }

            existing.FirstName = authorDto.FirstName;
            existing.LastName = authorDto.LastName;
            existing.DateOfBirth = dateOfBirth;
            existing.Country = authorDto.Country;

            await _repository.UpdateAsync(existing);
            return existing;
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _repository.GetByIdAsync(id);
            if (author == null)
            {
                throw new KeyNotFoundException($"Автор с ID {id} не найден");
            }

            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId)
            => await _repository.GetBooksByAuthorIdAsync(authorId);
    }

}
       

