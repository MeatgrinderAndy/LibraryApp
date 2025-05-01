using AutoMapper;
using FluentValidation;
using LibraryApp.Application.DTO.Author;
using LibraryApp.Application.Services.Interface;
using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Repository.Interface;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<AuthorDto> _validator;

        public AuthorService(IAuthorRepository repository, IMapper mapper, IValidator<AuthorDto> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<Author?> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<Author> CreateAsync(AuthorDto authorDto)
        {
            _validator.ValidateAndThrow(authorDto);
            var author = _mapper.Map<Author>(authorDto);
            await _repository.AddAsync(author);
            return author;
        }

        public async Task<Author?> UpdateAsync(int id, AuthorDto authorDto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.FirstName = authorDto.FirstName;
            existing.LastName = authorDto.LastName;
            existing.DateOfBirth = authorDto.DateOfBirth;
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


