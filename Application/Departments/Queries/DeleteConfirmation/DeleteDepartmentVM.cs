using AutoMapper;
using Application.Common.Mappings;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Departments.Queries.DeleteConfirmation
{
    public class DeleteDepartmentVM : IMapFrom<Department>
    {
        public int DepartmentID { get; set; }

        public string? Name { get; set; }

        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        public byte[]? RowVersion { get; set; }

        public string? AdministratorName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Department, DeleteDepartmentVM>()
                .ForMember(d => d.AdministratorName, opt => opt.MapFrom(s => s.Administrator != null ? s.Administrator.FullName : string.Empty));
        }
    }
}
