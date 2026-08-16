namespace Application.Instructors.Queries.GetInstructorsOverview
{
    public class InstructorsOverviewVM
    {
        public int? SelectedInstructorID { get; set; }

        public int? SelectedCourseID { get; set; }

        public List<InstructorVM> Instructors { get; set; }

        public List<EnrollmentVM> Enrollments { get; set; }

        public List<CourseVM> Courses { get; set; }
    }
}
