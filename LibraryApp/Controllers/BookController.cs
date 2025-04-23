using LibraryApp.Models;
using LibraryApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.DTO;
using System.IO;
using static System.Reflection.Metadata.BlobBuilder;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Repository.Interface;
using LibraryApp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using LibraryApp.Services;
using System.Text.Json;

namespace LibraryApp.Controllers
{
    [ApiController]
    [Route("api/books")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;
        private readonly AppDbContext _context;

        public BooksController(IBookService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]   
        public async Task<IActionResult> GetAll()
        {
            var bookDtos = from book in _context.Books
                                   join author in _context.Authors on book.AuthorId equals author.Id
                                   select new BookDto
                                   {
                                       Id = book.Id,
                                       Title = book.Title,
                                       Isbn = book.ISBN,
                                       Genre = book.Genre,
                                       AuthorId = book.AuthorId,
                                       AuthorName = author.LastName + " " + author.FirstName,
                                       DateWhenTaken = book.DateWhenTaken,
                                       CoverImage = book.CoverImage,
                                   };
            return Ok(bookDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _service.GetByIdAsync(id);
            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.ISBN,
                Description = book.Description,
                Genre = book.Genre,
                AuthorId = book.AuthorId,
                AuthorName = book.Author?.LastName + " " + book.Author.FirstName,
                DateWhenTaken = book.DateWhenTaken,
                DateWhenNeedToReturn = book.DateWhenNeedToReturn,
                CoverImage = book.CoverImage,
            };
            return book == null ? NotFound() : Ok(bookDto);
        }

        [HttpGet("isbn/{isbn}")]
        public async Task<IActionResult> GetByIsbn(string isbn)
        {
            var book = await _service.GetByIsbnAsync(isbn);
            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Isbn = book.ISBN,
                Genre = book.Genre,
                AuthorId = book.AuthorId,
                AuthorName = book.Author.LastName + " " + book.Author.FirstName,
                DateWhenTaken = book.DateWhenTaken,
                DateWhenNeedToReturn = book.DateWhenNeedToReturn,
                CoverImage = book.CoverImage,
            };
            return book == null ? NotFound() : Ok(bookDto);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyBooks()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var books = await _service.GetBooksAsync();
            var myBooks = books.Where(b => b.UserId == userId);

            var dtos = myBooks.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Isbn = b.ISBN,
                DateWhenTaken = b.DateWhenTaken,
                DateWhenNeedToReturn = b.DateWhenNeedToReturn
            });

            return Ok(dtos);
        }

        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var book = await _service.GetByIdAsync(id);
            if (book == null) return NotFound();

            book.UserId = null;
            book.DateWhenTaken = null;
            book.DateWhenNeedToReturn = null;

            await _service.UpdateAsync(book);
            return Ok("Книга возвращена");
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var form = await Request.ReadFormAsync();
                var bookDto = form["bookDto"];
                var coverImage = form.Files["coverImage"];

                var dto = JsonSerializer.Deserialize<CreateUpdateBookDto>(bookDto, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var book = new Book
                {
                    Title = dto.Title,
                    ISBN = dto.ISBN,
                    Genre = dto.Genre,
                    Description = dto.Description,
                    AuthorId = dto.AuthorId
                };

                if (coverImage != null && coverImage.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await coverImage.CopyToAsync(ms);
                    book.CoverImage = ms.ToArray();
                }

                await _service.AddAsync(book);

                return Ok(new
                {
                    book.Id,
                    book.AuthorId,
                    book.Title,
                    book.ISBN,
                    book.Genre,
                    book.Description
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateBook(int id)
        {
            try
            {
                var form = await Request.ReadFormAsync();
                var bookDto = form["bookDto"];
                var coverImage = form.Files["coverImage"];

                var dto = JsonSerializer.Deserialize<CreateUpdateBookDto>(bookDto, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var book = await _context.Books
                    .Include(b => b.Author) 
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (book == null) return NotFound();

                book.Title = dto.Title;
                book.ISBN = dto.ISBN;
                book.Genre = dto.Genre;
                book.Description = dto.Description;
                book.AuthorId = dto.AuthorId;

                if (coverImage != null && coverImage.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await coverImage.CopyToAsync(ms);
                    book.CoverImage = ms.ToArray();
                }

                await _service.UpdateAsync(book);

                string authorName = book.Author != null
                    ? $"{book.Author.FirstName} {book.Author.LastName}"
                    : "Неизвестный автор";

                return Ok(new
                {
                    book.Id,
                    book.Title,
                    book.ISBN,
                    book.Genre,
                    book.Description,
                    AuthorName = authorName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/borrow")]
        [Authorize]
        public async Task<IActionResult> BorrowBook(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var book = await _service.GetByIdAsync(id);
            if (book == null) return NotFound();

            if (book.DateWhenTaken != null)
                return BadRequest("Книга уже взята");

            book.UserId = userId;
            book.DateWhenTaken = DateOnly.FromDateTime(DateTime.UtcNow);
            book.DateWhenNeedToReturn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));

            await _service.UpdateAsync(book);
            return Ok("Книга одолжена");
        }

        [HttpPost("{id}/upload-image")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UploadImage(int id, [FromForm] IFormFile image)
        {
            var book = await _service.GetByIdAsync(id);
            if (book == null) return NotFound();

            if (image != null && image.Length > 0)
            {
                using var ms = new MemoryStream();
                await image.CopyToAsync(ms);
                book.CoverImage = ms.ToArray();
                await _service.UpdateAsync(book);
            }

            return Ok("Image uploaded");
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetImage(int id)
        {
            var book = await _service.GetByIdAsync(id);
            if (book == null || book.CoverImage == null)
                return NotFound();

            return File(book.CoverImage, "image/jpeg");
        }

        
    }

}
