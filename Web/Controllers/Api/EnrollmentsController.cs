using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Application.Common.Interfaces;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly ISchoolContext _context;
        public EnrollmentsController(ISchoolContext context) => _context = context;

        [HttpGet]
        public IActionResult Get()
        {
            var enrollments = _context.Enrollments.Select(e => new {
                e.Id,
                e.CourseID,
                e.StudentID,
                e.Grade
            }).ToList();

            return Ok(enrollments);
        }
    }
}

