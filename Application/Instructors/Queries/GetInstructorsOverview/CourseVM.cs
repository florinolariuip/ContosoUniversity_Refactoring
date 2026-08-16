using AutoMapper;
using Application.Common.Mappings;

namespace Application.Instructors.Queries.GetInstructorsOverview
{
    public class CourseVM : IMapFrom<Domain.Entities.CourseAssignment>
    {
        public int CourseID { get; set; }

        public string? Title { get; set; }

        public string? DepartmentName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.CourseAssignment, CourseVM>()
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Course.Title))
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Course.Department.Name));
        }
    }
}
