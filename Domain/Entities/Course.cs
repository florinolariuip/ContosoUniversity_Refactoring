using Domain.Entities.Common;

namespace Domain.Entities
{
    public class Course : BaseEntity
    {
        public string? Title { get; set; }
        public int Credits { get; set; }
        public int DepartmentID { get; set; }

        public Department? Department { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
        public ICollection<CourseAssignment> CourseAssignments { get; set; } = new HashSet<CourseAssignment>();
    }
}
