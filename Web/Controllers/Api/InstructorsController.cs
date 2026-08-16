using Application.Instructors.Commands.CreateInstructor;
using Application.Instructors.Commands.DeleteInstructor;
using Application.Instructors.Commands.UpdateInstructor;
using Application.Instructors.Queries.GetInstructorDetails;
using Application.Instructors.Queries.GetInstructorsOverview;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InstructorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetInstructors()
        {
            var result = await _mediator.Send(new GetInstructorsOverviewQuery(null, null));
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetInstructor(int id)
        {
            var result = await _mediator.Send(new GetInstructorDetailsQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInstructorCommand command)
        {
            await _mediator.Send(command);
            return CreatedAtAction(nameof(GetInstructors), null);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInstructorCommand command)
        {
            command.InstructorID = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteInstructorCommand(id));
            return NoContent();
        }
    }
}
