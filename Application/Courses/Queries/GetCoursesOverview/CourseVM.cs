using AutoMapper;
using Application.Common.Mappings;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Courses.Queries.GetCoursesOverview
{
    public class CourseVM : IMapFrom<Course>
    {
        [Display(Name = "Number")]
        public int CourseID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        public string? DepartmentName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Course, CourseVM>()
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Department.Name));
        }
    }
}
