using AutoMapper;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Courses.Queries.GetCourseList
{
    public class CourseSelectionVM : IMapFrom<Course>
    {
        public int CourseID { get; set; }

        public string? Title { get; set; }

        public bool Assigned { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Course, CourseSelectionVM>()
                .ForMember(d => d.Assigned, opt => opt.Ignore());
        }
    }
}
