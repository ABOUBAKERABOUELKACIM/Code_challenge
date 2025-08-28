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
    public class AssistantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssistantController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<ActionResult<List<AssistantDto>>> GetAssistants()
        {
            var assistants = await _mediator.Send(new GetAssistantsQuery());
            return Ok(assistants);
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AssistantDto>> GetAssistant(int id)
        {
            var assistant = await _mediator.Send(new GetAssistantByIdQuery(id));

            if (assistant == null)
            {
                return NotFound($"Assistant with ID {id} not found.");
            }

            return Ok(assistant);
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<List<AssistantDto>>> GetAssistantsByDoctor(int doctorId)
        {
            var assistants = await _mediator.Send(new GetAssistantsByDoctorQuery(doctorId));
            return Ok(assistants);
        }
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<ActionResult<AssistantDto>> CreateAssistant([FromBody] CreateAssistantDto createAssistantDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new CreateAssistantCommand(createAssistantDto);
            var createdAssistant = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetAssistant),
                new { id = createdAssistant.Id },
                createdAssistant);
        }
        [Authorize(Roles = "Doctor")]
        [HttpPut("{id}")]
        public async Task<ActionResult<AssistantDto>> UpdateAssistant(int id, [FromBody] CreateAssistantDto updateAssistantDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new UpdateAssistantCommand(id, updateAssistantDto);
            var updatedAssistant = await _mediator.Send(command);

            if (updatedAssistant == null)
            {
                return NotFound($"Assistant with ID {id} not found.");
            }

            return Ok(updatedAssistant);
        }
        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAssistant(int id)
        {
            var success = await _mediator.Send(new DeleteAssistantCommand(id));

            if (!success)
            {
                return NotFound($"Assistant with ID {id} not found.");
            }

            return NoContent();
        }
    }
}