using MediatR;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Queries
{
    public record GetDoctorsQuery : IRequest<List<DoctorDto>>;

    public record GetDoctorByIdQuery(int Id) : IRequest<DoctorDto?>;

    public record GetDoctorsByOfficeQuery(int OfficeId) : IRequest<List<DoctorDto>>;
}