using FluentValidation;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Validators
{
    public class CreateDoctorValidator : AbstractValidator<CreateDoctorDto>
    {
        public CreateDoctorValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(200).WithMessage("Email cannot exceed 200 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)").WithMessage("Password must contain uppercase, lowercase, and number");

            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Specialization is required")
                .MaximumLength(200).WithMessage("Specialization cannot exceed 200 characters");

            RuleFor(x => x.OfficeId)
                .GreaterThan(0).WithMessage("Valid office must be selected");
        }
    }
}