using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ISchoolContext _context;
        public CoursesController(ISchoolContext context) => _context = context;

        [HttpGet]
        public IActionResult Get()
        {
            var courses = _context.Courses.Select(c => new {
                c.Id,
                c.Title,
                c.Credits
            }).ToList();

            return Ok(courses);
        }
    }
}
