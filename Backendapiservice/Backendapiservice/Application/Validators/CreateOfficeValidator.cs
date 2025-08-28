using FluentValidation;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Validators
{
	public class CreateOfficeValidator : AbstractValidator<CreateOfficeDto>
	{
		public CreateOfficeValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Office name is required")
				.MaximumLength(200).WithMessage("Office name cannot exceed 200 characters");

			RuleFor(x => x.Address)
				.NotEmpty().WithMessage("Address is required")
				.MaximumLength(500).WithMessage("Address cannot exceed 500 characters");

			RuleFor(x => x.Phone)
				.NotEmpty().WithMessage("Phone number is required")
				.MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters")
				.Matches(@"^\+?[\d\s\-\(\)]+$").WithMessage("Phone number format is invalid");
		}
	}
}