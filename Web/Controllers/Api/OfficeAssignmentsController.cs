using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfficeAssignmentsController : ControllerBase
    {
        private readonly ISchoolContext _context;

        public OfficeAssignmentsController(ISchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOfficeAssignments()
        {
            var items = await _context.OfficeAssignments
                .Include(o => o.Instructor)
                .ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOfficeAssignment(int id)
        {
            var item = await _context.OfficeAssignments
                .Include(o => o.Instructor)
                .FirstOrDefaultAsync(o => o.InstructorID == id);

            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OfficeAssignment officeAssignment)
        {
            _context.OfficeAssignments.Add(officeAssignment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOfficeAssignment), new { id = officeAssignment.InstructorID }, officeAssignment);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OfficeAssignment officeAssignment)
        {
            if (id != officeAssignment.InstructorID) return BadRequest();

            _context.Entry(officeAssignment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.OfficeAssignments.FindAsync(id);
            if (item == null) return NotFound();

            _context.OfficeAssignments.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

