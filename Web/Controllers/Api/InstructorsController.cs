using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Application.Common.Interfaces;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorsController : ControllerBase
    {
        private readonly ISchoolContext _context;
        public InstructorsController(ISchoolContext context) => _context = context;

        [HttpGet]
        public IActionResult Get()
        {
            var instructors = _context.Instructors.Select(i => new {
                i.Id,
                i.FirstMidName,
                i.LastName,
                i.HireDate
            }).ToList();

            return Ok(instructors);
        }
    }
}

