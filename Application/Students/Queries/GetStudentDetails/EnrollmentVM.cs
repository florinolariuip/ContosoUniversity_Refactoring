using AutoMapper;
using Application.Common.Mappings;
using Domain.Entities;
using Domain.Enums;

namespace Application.Students.Queries.GetStudentDetails
{
    public class EnrollmentVM : IMapFrom<Enrollment>
    {
        public string CourseTitle { get; set; }

        public Grade? Grade { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Enrollment, EnrollmentVM>()
                .ForMember(d => d.CourseTitle, opt => opt.MapFrom(s => s.Course.Title))
                .ForMember(d => d.Grade, opt => opt.MapFrom(s => s.Grade));
        }
    }
}