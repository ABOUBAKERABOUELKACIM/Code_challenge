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
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpGet]
        public async Task<ActionResult<List<PatientDto>>> GetPatients()
        {
            var patients = await _mediator.Send(new GetPatientsQuery());
            return Ok(patients);
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatient(int id)
        {
            var patient = await _mediator.Send(new GetPatientByIdQuery(id));

            if (patient == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }

            return Ok(patient);
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpPost]
        public async Task<ActionResult<PatientDto>> CreatePatient([FromBody] CreatePatientDto createPatientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new CreatePatientCommand(createPatientDto);
            var createdPatient = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetPatient),
                new { id = createdPatient.Id },
                createdPatient);
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpPut("{id}")]
        public async Task<ActionResult<PatientDto>> UpdatePatient(int id, [FromBody] CreatePatientDto updatePatientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new UpdatePatientCommand(id, updatePatientDto);
            var updatedPatient = await _mediator.Send(command);

            if (updatedPatient == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }

            return Ok(updatedPatient);
        }
        [Authorize(Roles = "Assistant,Doctor")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            try
            {
                var success = await _mediator.Send(new DeletePatientCommand(id));

                if (!success)
                {
                    return NotFound($"Patient with ID {id} not found.");
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