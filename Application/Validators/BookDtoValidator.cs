using FluentValidation;
using LibraryApp.Application.DTO.Book;

namespace LibraryApp.Application.Validators
{
    public class BookDtoValidator : AbstractValidator<BookDto>
    {
        public BookDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название книги обязательно")
                .MaximumLength(200).WithMessage("Название не должно превышать 200 символов");

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN обязателен")
                .Length(13, 15).WithMessage("ISBN должен содержать от 13 до 15 символов")
                .Matches(@"^[0-9\-]+$").WithMessage("ISBN должен содержать только цифры и дефисы");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Описание обязательно")
                .MaximumLength(1000).WithMessage("Описание не должно превышать 1000 символов");

            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Жанр обязателен")
                .MaximumLength(100).WithMessage("Жанр не должен превышать 100 символов")
                .Matches(@"^[a-zA-Zа-яА-Я\s\-]+$")
                    .WithMessage("Жанр должен содержать только буквы и дефисы");

            RuleFor(x => x.AuthorId)
                .NotEmpty().WithMessage("Автор обязателен")
                .GreaterThan(0).WithMessage("Неверный идентификатор автора");

            RuleFor(x => x.CoverImage)
                .Must(image => image == null || image.Length <= 2_000_000)
                    .WithMessage("Размер обложки не должен превышать 2MB")
                .When(x => x.CoverImage != null);

            RuleFor(x => x.DateWhenTaken)
                .LessThanOrEqualTo(x => x.DateWhenNeedToReturn)
                    .WithMessage("Дата взятия должна быть раньше даты возврата")
                .When(x => x.DateWhenTaken.HasValue && x.DateWhenNeedToReturn.HasValue);
        }
    }
}
