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
    public class OfficeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OfficeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<OfficeDto>>> GetOffices()
        {
            var offices = await _mediator.Send(new GetOfficesQuery());
            return Ok(offices);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<OfficeDto>> GetOffice(int id)
        {
            var office = await _mediator.Send(new GetOfficeByIdQuery(id));

            if (office == null)
            {
                return NotFound($"Office with ID {id} not found.");
            }

            return Ok(office);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<OfficeDto>> CreateOffice([FromBody] CreateOfficeDto createOfficeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new CreateOfficeCommand(createOfficeDto);
            var createdOffice = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetOffice),
                new { id = createdOffice.Id },
                createdOffice);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<OfficeDto>> UpdateOffice(int id, [FromBody] CreateOfficeDto updateOfficeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new UpdateOfficeCommand(id, updateOfficeDto);
            var updatedOffice = await _mediator.Send(command);

            if (updatedOffice == null)
            {
                return NotFound($"Office with ID {id} not found.");
            }

            return Ok(updatedOffice);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOffice(int id)
        {
            try
            {
                var success = await _mediator.Send(new DeleteOfficeCommand(id));

                if (!success)
                {
                    return NotFound($"Office with ID {id} not found.");
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