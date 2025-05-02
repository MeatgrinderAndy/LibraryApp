using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LibraryApp.Application.Services.Interface;
using LibraryApp.Application.DTO.Book;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryApp.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _service.GetByIdAsync(id);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpGet("isbn/{isbn}")]
        public async Task<IActionResult> GetByIsbn(string isbn)
        {
            var book = await _service.GetByIsbnAsync(isbn);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyBooks()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            return Ok(await _service.GetMyBooksAsync(userId));
        }

        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            await _service.ReturnBookAsync(id);
            return Ok("Книга возвращена");
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [Consumes("multipart/form-data")] //оставил чтоб сразу с картинкой отправлять
        public async Task<IActionResult> Create([FromForm] CreateUpdateBookDto dto, IFormFile? coverImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _service.AddAsync(dto, coverImage);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [Consumes("multipart/form-data")] 
        public async Task<IActionResult> UpdateBook(int id, [FromForm] CreateUpdateBookDto dto, IFormFile? coverImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _service.UpdateAsync(id, dto, coverImage);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/borrow")]
        [Authorize]
        public async Task<IActionResult> BorrowBook(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            await _service.BorrowBookAsync(id, userId);
            return Ok("Книга одолжена");
        }

        [SwaggerIgnore]
        [HttpPost("{id}/upload-image")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UploadImage(int id, [FromForm] IFormFile image)
        {
            await _service.UploadImageAsync(id, image);
            return Ok("Image uploaded");
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = await _service.GetImageAsync(id);
            if (image == null)
                return NotFound();
            return File(image, "image/jpeg");
        }

        [HttpGet("page/{page}/rows/{rows}")]
        public async Task<IActionResult> GetBooksOnPage(int page, int rows)
        {
            return Ok(await _service.GetBooksPageAsync(page, rows));
        }
    }


}
