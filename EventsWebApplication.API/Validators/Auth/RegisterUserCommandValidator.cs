using EventsWebApplication.Application.UseCases.AuthUseCases.Commands.RegisterUser;
using FluentValidation;

namespace EventsWebApplication.API.Validators.Auth
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator() 
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name must be less than 50 characters");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required")
                .MaximumLength(50).WithMessage("Surname must be less than 50 characters");

            RuleFor(x => x.Birthday)
                .Must(BeAtLeast18YearsOld).WithMessage("You must be at least 18 years old");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match");

        }

        private bool BeAtLeast18YearsOld(DateOnly birthday)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var minBirthDate = today.AddYears(-18);
            return birthday <= minBirthDate;
        }

    }
}
