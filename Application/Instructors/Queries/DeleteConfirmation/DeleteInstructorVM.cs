using AutoMapper;
using Application.Common.Mappings;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Instructors.Queries.DeleteConfirmation
{
    public class DeleteInstructorVM : IMapFrom<Instructor>
    {
        public int InstructorID { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Instructor, DeleteInstructorVM>()
                .ForMember(d => d.InstructorID, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.FirstMidName));
        }
    }
}
