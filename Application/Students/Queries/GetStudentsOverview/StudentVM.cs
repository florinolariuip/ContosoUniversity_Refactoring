using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Students.Queries.GetStudentsOverview
{
    public class StudentVM : Common.Mappings.IMapFrom<Student>
    {
        public int StudentID { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Student, StudentVM>()
                .ForMember(d => d.StudentID, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.FirstMidName));
        }
    }
}
