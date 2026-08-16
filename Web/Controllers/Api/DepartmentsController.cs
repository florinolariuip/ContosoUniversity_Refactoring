using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Application.Common.Interfaces;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly ISchoolContext _context;
        public DepartmentsController(ISchoolContext context) => _context = context;

        [HttpGet]
        public IActionResult Get()
        {
            var departments = _context.Departments.Select(d => new {
                d.Id,
                d.Name,
                d.Budget
            }).ToList();

            return Ok(departments);
        }
    }
}

