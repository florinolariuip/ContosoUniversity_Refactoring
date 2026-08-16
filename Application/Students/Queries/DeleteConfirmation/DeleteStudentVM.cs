using AutoMapper;
using Application.Common.Mappings;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Students.Queries.DeleteConfirmation
{
    public class DeleteStudentVM : IMapFrom<Student>
    {
        public int StudentID { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Student, DeleteStudentVM>()
                .ForMember(d => d.StudentID, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.FirstMidName));
        }
    }
}
