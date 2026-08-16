using MediatR;

namespace Application.Students.Commands.CreateStudent
{
    public class CreateStudentCommand : IRequest
    {
        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public DateTime EnrollmentDate { get; set; }
    }
}
