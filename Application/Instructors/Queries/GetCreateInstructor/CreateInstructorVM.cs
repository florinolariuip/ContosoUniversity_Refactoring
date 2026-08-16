using System.ComponentModel.DataAnnotations;

namespace Application.Instructors.Queries.GetCreateInstructor
{
    public class CreateInstructorVM
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }
    }
}
