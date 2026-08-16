using MediatR;

namespace Application.CourseAssignment.Commands
{
    public class UpdateCourseAssignmentsCommand : IRequest
    {
        public int? InstructorID { get; set; }

        public string[] SelectedCourses { get; set; }

        public UpdateCourseAssignmentsCommand(int instructorID, string[] selectedCourses)
        {
            this.InstructorID = instructorID;
            this.SelectedCourses = selectedCourses;
        }
    }
}
