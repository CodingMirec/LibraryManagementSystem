using FluentValidation;

namespace LibraryManagementSystem.Core.DTOs.Validators
{
    public class BookDTOValidator : AbstractValidator<BookDTO>
    {
        public BookDTOValidator()
        {
            RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be a valid positive integer.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(2, 100).WithMessage("Title must be between 2 and 100 characters.");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author is required.")
                .Length(2, 50).WithMessage("Author must be between 2 and 50 characters.");

            RuleFor(x => x.PublishedDate)
                .NotEmpty().WithMessage("Published date is required.")
                .Must(date => date != default(DateTime)).WithMessage("LoanDate must be a valid date.");
        }
    }
}
