using FluentValidation;
using LibraryManagementSystem.Core.DTOs.RequestDtos;

namespace LibraryManagementSystem.Core.DTOs.Validators
{
    public class LoanDTOValidator : AbstractValidator<LoanRequestDto>
    {
        public LoanDTOValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("Book ID must be greater than zero.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than zero.");

            RuleFor(x => x.LoanDate)
                .NotEmpty().WithMessage("Loan date is required.")
                .Must(date => date != default(DateTime)).WithMessage("LoanDate must be a valid date.");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Due date is required.")
                .Must(date => date != default(DateTime)).WithMessage("LoanDate must be a valid date.")
                .GreaterThan(x => x.LoanDate).WithMessage("Due date must be after loan date.");

            RuleFor(x => x.ReturnDate)
                .GreaterThan(x => x.LoanDate)
                .When(x => x.ReturnDate.HasValue)
                .WithMessage("ReturnDate must be after LoanDate when provided.")
                .Must(date => !date.HasValue || date.Value != default(DateTime)).WithMessage("ReturnDate must be a valid date when provided.");
        }
    }
}
