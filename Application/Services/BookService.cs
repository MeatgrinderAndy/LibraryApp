using LibraryApp.Infrastructure.Repository.Interface;
using LibraryApp.Application.Services.Interface;
using AutoMapper;
using LibraryApp.Application.DTO.Book;
using FluentValidation;
using LibraryApp.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace LibraryApp.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;
        private readonly IMapper _mapper;
        private readonly IValidator<BookDto> _validator;

        public BookService(IBookRepository repo, IMapper mapper, IValidator<BookDto> validator)
        {
            _repo = repo;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto?> GetByIdAsync(int id)
        {
            var book = await _repo.GetByIdAsync(id);
            return book == null ? null : _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto?> GetByIsbnAsync(string isbn)
        {
            var book = await _repo.GetByIsbnAsync(isbn);
            return book == null ? null : _mapper.Map<BookDto>(book);
        }

        public async Task<IEnumerable<BookDto>> GetMyBooksAsync(int userId)
        {
            var books = await _repo.GetAllAsync();
            var myBooks = books.Where(b => b.UserId == userId);
            return _mapper.Map<IEnumerable<BookDto>>(myBooks);
        }

        public async Task<BookDto> AddAsync(CreateUpdateBookDto dto, IFormFile? coverImage)
        {
            var book = _mapper.Map<Book>(dto);
            _validator.ValidateAndThrow(_mapper.Map<BookDto>(book));
            if (coverImage != null && coverImage.Length > 0)
            {
                using var ms = new MemoryStream();
                await coverImage.CopyToAsync(ms);
                book.CoverImage = ms.ToArray();
            }

            await _repo.AddAsync(book);

            return _mapper.Map<BookDto>(book);
        }

        public async Task UpdateAsync(int id, CreateUpdateBookDto dto, IFormFile? coverImage)
        {
            var book = await _repo.GetByIdAsync(id);
            if (book == null) throw new Exception("Book not found");

            _mapper.Map(dto, book);
            _validator.ValidateAndThrow(_mapper.Map<BookDto>(book));
            if (coverImage != null && coverImage.Length > 0)
            {
                using var ms = new MemoryStream();
                await coverImage.CopyToAsync(ms);
                book.CoverImage = ms.ToArray();
            }

            await _repo.UpdateAsync(book);
        }

        public async Task BorrowBookAsync(int id, int userId)
        {
            var book = await _repo.GetByIdAsync(id);
            if (book == null) throw new Exception("Book not found");

            if (book.DateWhenTaken != null)
                throw new Exception("Книга уже взята");

            book.UserId = userId;
            book.DateWhenTaken = DateOnly.FromDateTime(DateTime.UtcNow);
            book.DateWhenNeedToReturn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));

            await _repo.UpdateAsync(book);
        }

        public async Task ReturnBookAsync(int id)
        {
            var book = await _repo.GetByIdAsync(id);
            if (book == null) throw new Exception("Book not found");

            book.UserId = null;
            book.DateWhenTaken = null;
            book.DateWhenNeedToReturn = null;

            await _repo.UpdateAsync(book);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<byte[]?> GetImageAsync(int id)
        {
            var book = await _repo.GetByIdAsync(id);
            return book?.CoverImage;
        }

        public async Task UploadImageAsync(int id, IFormFile image)
        {
            var book = await _repo.GetByIdAsync(id);
            if (book == null) throw new Exception("Book not found");

            if (image != null && image.Length > 0)
            {
                using var ms = new MemoryStream();
                await image.CopyToAsync(ms);
                book.CoverImage = ms.ToArray();
                await _repo.UpdateAsync(book);
            }
        }

        public async Task<IEnumerable<BookDto>> GetBooksPageAsync(int pageNumber, int rowsNumber)
        {
            var booksOnPage = await _repo.GetPagedAsync(pageNumber, rowsNumber);
            return _mapper.Map<IEnumerable<BookDto>>(booksOnPage);

        }

    }

}
