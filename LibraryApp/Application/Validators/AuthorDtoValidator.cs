using FluentValidation;
using LibraryApp.Application.DTO.Author;
using System;

namespace LibraryApp.Application.Validators
{


    public class AuthorDtoValidator : AbstractValidator<AuthorDto>
    {
        public AuthorDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Имя обязательно")
                .MaximumLength(50).WithMessage("Имя не должно превышать 50 символов");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Фамилия обязательна")
                .MaximumLength(50).WithMessage("Фамилия не должна превышать 50 символов");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Дата рождения обязательна")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                    .WithMessage("Дата рождения не может быть в будущем");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Страна обязательна")
                .MaximumLength(50).WithMessage("Страна не должна превышать 50 символов")
                .Matches(@"^[a-zA-Zа-яА-Я\s\-]+$")
                    .WithMessage("Страна должна содержать только буквы и дефисы");
        }
    }
}
