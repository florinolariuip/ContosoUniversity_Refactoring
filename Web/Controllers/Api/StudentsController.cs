using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Application.Common.Interfaces;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ISchoolContext _context;
        public StudentsController(ISchoolContext context) => _context = context;

        [HttpGet]
        public IActionResult Get()
        {
            var students = _context.Students.Select(s => new {
                s.Id,
                s.FirstMidName,
                s.LastName,
                s.EnrollmentDate
            }).ToList();

            return Ok(students);
        }
    }
}

