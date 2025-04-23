using LibraryApp.Models;
using LibraryApp.Repository.Interface;
using LibraryApp.Services;
using LibraryApp.DTO;
using Moq;
using Xunit;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Tests.Services
{
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorRepository> _mockRepo;
        private readonly AuthorService _service;

        public AuthorServiceTests()
        {
            _mockRepo = new Mock<IAuthorRepository>();
            _service = new AuthorService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldValidateDateOfBirth()
        {
            var invalidDto = new AuthorDto
            {
                FirstName = "Test",
                LastName = "Author",
                DateOfBirth = "invalid-date",
                Country = "Country"
            };

            await Assert.ThrowsAsync<ValidationException>(() => _service.CreateAsync(invalidDto));
        }

        [Fact]
        public async Task CreateAsync_ShouldCallRepository()
        {
            var validDto = new AuthorDto
            {
                FirstName = "Test",
                LastName = "Author",
                DateOfBirth = "2000-01-01",
                Country = "Country"
            };
            _mockRepo.Setup(x => x.AddAsync(It.IsAny<Author>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _service.CreateAsync(validDto);

            _mockRepo.Verify();
            Assert.Equal("Test", result.FirstName);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenAuthorNotFound()
        {
            _mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Author?)null);
            
            var result = await _service.UpdateAsync(1, new AuthorDto());

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenAuthorNotFound()
        {
            _mockRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Author?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(1));
        }
    }
}