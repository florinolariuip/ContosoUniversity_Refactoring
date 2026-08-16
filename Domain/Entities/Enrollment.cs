using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public Grade? Grade { get; set; }

        public Course? Course { get; set; }
        public Student? Student { get; set; }
    }
}
