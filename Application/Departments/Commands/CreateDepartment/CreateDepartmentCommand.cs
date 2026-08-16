using MediatR;

namespace Application.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommand : IRequest
    {
        public string? Name { get; set; }

        public decimal Budget { get; set; }

        public DateTime StartDate { get; set; }

        public int InstructorID { get; set; }
    }
}
