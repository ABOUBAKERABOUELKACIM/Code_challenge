using MediatR;
using Microsoft.AspNetCore.Mvc;
using Backendapiservice.Application.Commands;
using Backendapiservice.Application.Queries;
using Backendapiservice.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Backendapiservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpGet]
        public async Task<ActionResult<List<AppointmentDto>>> GetAppointments()
        {
            var appointments = await _mediator.Send(new GetAppointmentsQuery());
            return Ok(appointments);
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointment(int id)
        {
            var appointment = await _mediator.Send(new GetAppointmentByIdQuery(id));

            if (appointment == null)
            {
                return NotFound($"Appointment with ID {id} not found.");
            }

            return Ok(appointment);
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<List<AppointmentDto>>> GetAppointmentsByDoctor(int doctorId)
        {
            var appointments = await _mediator.Send(new GetAppointmentsByDoctorQuery(doctorId));
            return Ok(appointments);
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<List<AppointmentDto>>> GetAppointmentsByPatient(int patientId)
        {
            var appointments = await _mediator.Send(new GetAppointmentsByPatientQuery(patientId));
            return Ok(appointments);
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpGet("daterange")]
        public async Task<ActionResult<List<AppointmentDto>>> GetAppointmentsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var appointments = await _mediator.Send(new GetAppointmentsByDateRangeQuery(startDate, endDate));
            return Ok(appointments);
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> CreateAppointment([FromBody] CreateAppointmentDto createAppointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new CreateAppointmentCommand(createAppointmentDto);
            var createdAppointment = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetAppointment),
                new { id = createdAppointment.Id },
                createdAppointment);
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpPut("{id}")]
        public async Task<ActionResult<AppointmentDto>> UpdateAppointment(int id, [FromBody] CreateAppointmentDto updateAppointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new UpdateAppointmentCommand(id, updateAppointmentDto);
            var updatedAppointment = await _mediator.Send(command);

            if (updatedAppointment == null)
            {
                return NotFound($"Appointment with ID {id} not found.");
            }

            return Ok(updatedAppointment);
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpPut("{id}/status")]
        public async Task<ActionResult<AppointmentDto>> UpdateAppointmentStatus(int id, [FromBody] UpdateAppointmentStatusDto statusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new UpdateAppointmentStatusCommand(id, statusDto);
            var updatedAppointment = await _mediator.Send(command);

            if (updatedAppointment == null)
            {
                return NotFound($"Appointment with ID {id} not found.");
            }

            return Ok(updatedAppointment);
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAppointment(int id)
        {
            var success = await _mediator.Send(new DeleteAppointmentCommand(id));

            if (!success)
            {
                return NotFound($"Appointment with ID {id} not found.");
            }

            return NoContent();
        }
    }
}