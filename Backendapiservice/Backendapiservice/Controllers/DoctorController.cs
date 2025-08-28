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
    public class DoctorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DoctorController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<DoctorDto>>> GetDoctors()
        {
            var doctors = await _mediator.Send(new GetDoctorsQuery());
            return Ok(doctors);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
        {
            var doctor = await _mediator.Send(new GetDoctorByIdQuery(id));

            if (doctor == null)
            {
                return NotFound($"Doctor with ID {id} not found.");
            }

            return Ok(doctor);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("office/{officeId}")]
        public async Task<ActionResult<List<DoctorDto>>> GetDoctorsByOffice(int officeId)
        {
            var doctors = await _mediator.Send(new GetDoctorsByOfficeQuery(officeId));
            return Ok(doctors);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<DoctorDto>> CreateDoctor([FromBody] CreateDoctorDto createDoctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new CreateDoctorCommand(createDoctorDto);
            var createdDoctor = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetDoctor),
                new { id = createdDoctor.Id },
                createdDoctor);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<DoctorDto>> UpdateDoctor(int id, [FromBody] CreateDoctorDto updateDoctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new UpdateDoctorCommand(id, updateDoctorDto);
            var updatedDoctor = await _mediator.Send(command);

            if (updatedDoctor == null)
            {
                return NotFound($"Doctor with ID {id} not found.");
            }

            return Ok(updatedDoctor);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            try
            {
                var success = await _mediator.Send(new DeleteDoctorCommand(id));

                if (!success)
                {
                    return NotFound($"Doctor with ID {id} not found.");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}