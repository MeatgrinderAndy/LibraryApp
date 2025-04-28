using LibraryApp.Application.Services.Interface;
using LibraryApp.Application.Services;
using LibraryApp.Auth.Interface;
using LibraryApp.Auth;
using LibraryApp.Infrastructure.Repository.Interface;
using LibraryApp.Infrastructure.Repository;
using LibraryApp.Application.Validators;
using LibraryApp.Services;
using LibraryApp.Application.DTO.Author;
using LibraryApp.Application.DTO.Book;
using FluentValidation;

namespace LibraryApp.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IValidator<AuthorDto>, AuthorDtoValidator>();
            services.AddScoped<IValidator<BookDto>, BookDtoValidator>();
            services.AddMemoryCache();

            return services;
        }
    }
}
