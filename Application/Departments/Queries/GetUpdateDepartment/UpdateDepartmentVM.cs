using AutoMapper;
using Application.Common.Mappings;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Departments.Queries.GetUpdateDepartment
{
    public class UpdateDepartmentVM : IMapFrom<Department>
    {
        public int DepartmentID { get; set; }

        public string? Name { get; set; }

        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        public byte[]? RowVersion { get; set; }

        public int? InstructorID { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Department, UpdateDepartmentVM>();
        }
    }
}
