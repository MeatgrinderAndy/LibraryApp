using LibraryApp.Application.DTO.Author;
using Moq;
using LibraryApp.Application.Services;
using LibraryApp.Infrastructure.Repository.Interface;
using LibraryApp.Application.Mapper;
using AutoMapper;
using FluentValidation;
using LibraryApp.Application.Validators;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Tests.Services
{
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorRepository> _mockRepo;
        private readonly IMapper _mapper;
        private readonly AuthorService _service;
        private readonly IValidator<AuthorDto> _validator;

        public AuthorServiceTests()
        {
            _mockRepo = new Mock<IAuthorRepository>();
            _validator = new AuthorDtoValidator();
            _mapper = CreateTestMapper(); 

            _service = new AuthorService(_mockRepo.Object, _mapper, _validator);
        }

        private IMapper CreateTestMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile<MappingProfile>());

            return configuration.CreateMapper();
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowExceptionOnDate()
        {
            var invalidDto = new AuthorDto
            {
                FirstName = "Test",
                LastName = "Author",
                DateOfBirth = DateOnly.ParseExact("2050-01-01", "yyyy-MM-dd"),
                Country = "Country"
            };

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _service.CreateAsync(invalidDto));
        }

        [Fact]
        public async Task CreateAsync_ShouldCallRepository()
        {
            var validDto = new AuthorDto
            {
                FirstName = "Test",
                LastName = "Author",
                DateOfBirth = DateOnly.ParseExact("2000-01-01", "yyyy-MM-dd"),
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