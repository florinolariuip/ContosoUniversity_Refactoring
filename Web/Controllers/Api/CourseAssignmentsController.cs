using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseAssignmentsController : ControllerBase
    {
        private readonly ISchoolContext _context;

        public CourseAssignmentsController(ISchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseAssignments()
        {
            var items = await _context.CourseAssignments
                .Include(ca => ca.Course)
                .Include(ca => ca.Instructor)
                .ToListAsync();
            return Ok(items);
        }

        [HttpGet("{instructorId:int}/{courseId:int}")]
        public async Task<IActionResult> GetCourseAssignment(int instructorId, int courseId)
        {
            var item = await _context.CourseAssignments
                .Include(ca => ca.Course)
                .Include(ca => ca.Instructor)
                .FirstOrDefaultAsync(ca => ca.InstructorID == instructorId && ca.CourseID == courseId);

            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseAssignment assignment)
        {
            _context.CourseAssignments.Add(assignment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCourseAssignment), new { instructorId = assignment.InstructorID, courseId = assignment.CourseID }, assignment);
        }

        [HttpDelete("{instructorId:int}/{courseId:int}")]
        public async Task<IActionResult> Delete(int instructorId, int courseId)
        {
            var item = await _context.CourseAssignments
                .FirstOrDefaultAsync(ca => ca.InstructorID == instructorId && ca.CourseID == courseId);

            if (item == null) return NotFound();

            _context.CourseAssignments.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

