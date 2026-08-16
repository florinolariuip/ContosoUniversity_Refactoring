using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Queries.GetCourseList
{
    public class GetCourseListQueryHandler : IRequestHandler<GetCourseListQuery, List<CourseSelectionVM>>
    {
        private readonly ISchoolContext _context;
        private readonly IMapper _mapper;

        public GetCourseListQueryHandler(ISchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CourseSelectionVM>> Handle(GetCourseListQuery request, CancellationToken cancellationToken)
        {
            var courses = await _context.Courses
                .AsNoTracking()
                .ProjectTo<CourseSelectionVM>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var courseIDsForInstructor = await _context.CourseAssignments
                .Where(x => x.InstructorID == request.InstructorID)
                .Select(x => x.CourseID).ToListAsync();

            foreach(var course in courses)
            {
                course.Assigned = courseIDsForInstructor.Contains(course.CourseID);
            }

            return courses;
        }
    }
}
