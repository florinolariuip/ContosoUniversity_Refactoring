using AutoMapper;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Instructors.Queries.GetInstructorsLookup
{
    public class InstructorLookupVM : IMapFrom<Instructor>
    {
        public int ID { get; set; }
        public string? FullName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Instructor, InstructorLookupVM>();
        }
    }
}
