using FluentValidation;
using LibraryManagementSystem.Core.DTOs.RequestDtos;

namespace LibraryManagementSystem.Core.DTOs.Validators
{
    public class UserDTOValidator : AbstractValidator<UserRequestDto>
    {
        public UserDTOValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Matches(@"^[a-zA-Z'-]+$").WithMessage("First name can contain only letters, apostrophes, hyphens, and spaces.")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Matches(@"^[a-zA-Z'-]+$").WithMessage("Last name can contain only letters, apostrophes, hyphens, and spaces.")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email address cannot exceed 100 characters.");
        }
    }
}
