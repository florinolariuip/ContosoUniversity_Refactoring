using System.ComponentModel.DataAnnotations;

namespace Application.Departments.Queries.GetCreateDepartment
{
    public class CreateDepartmentVM
    {
        public string? Name { get; set; }

        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        public int InstructorID { get; set; }
    }
}
