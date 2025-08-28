using MediatR;

namespace Backendapiservice.Application.Commands
{
    public record DeleteOfficeCommand(int Id) : IRequest<bool>;
    public record DeleteDoctorCommand(int Id) : IRequest<bool>;
    public record DeletePatientCommand(int Id) : IRequest<bool>;
    public record DeleteAssistantCommand(int Id) : IRequest<bool>;
    public record DeleteAppointmentCommand(int Id) : IRequest<bool>;
}