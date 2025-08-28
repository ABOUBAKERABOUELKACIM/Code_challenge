using FluentValidation;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Validators
{
    public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentValidator()
        {
            RuleFor(x => x.AppointmentDateTime)
                .NotEmpty().WithMessage("Appointment date and time is required")
                .GreaterThan(DateTime.Now).WithMessage("Appointment must be in the future");

            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("Valid doctor must be selected");

            RuleFor(x => x.PatientId)
                .GreaterThan(0).WithMessage("Valid patient must be selected");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters");
        }
    }
}