using MediatR;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Commands
{
    public record CreateAppointmentCommand(CreateAppointmentDto AppointmentData) : IRequest<AppointmentDto>;

    public record UpdateAppointmentCommand(int Id, CreateAppointmentDto AppointmentData) : IRequest<AppointmentDto?>;

    public record UpdateAppointmentStatusCommand(int Id, UpdateAppointmentStatusDto StatusData) : IRequest<AppointmentDto?>;
}