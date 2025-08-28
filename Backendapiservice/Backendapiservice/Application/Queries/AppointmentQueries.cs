using MediatR;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Queries
{
    public record GetAppointmentsQuery : IRequest<List<AppointmentDto>>;

    public record GetAppointmentByIdQuery(int Id) : IRequest<AppointmentDto?>;

    public record GetAppointmentsByDoctorQuery(int DoctorId) : IRequest<List<AppointmentDto>>;

    public record GetAppointmentsByPatientQuery(int PatientId) : IRequest<List<AppointmentDto>>;

    public record GetAppointmentsByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<List<AppointmentDto>>;
} 