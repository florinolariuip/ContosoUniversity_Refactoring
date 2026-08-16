using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Queries.GetCourseDetails
{
    public class GetCourseDetailsQueryHandler : IRequestHandler<GetCourseDetailsQuery, CourseDetailVM>
    {
        private readonly ISchoolContext _context;
        private readonly IMapper _mapper;

        public GetCourseDetailsQueryHandler(ISchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CourseDetailVM> Handle(GetCourseDetailsQuery request, CancellationToken cancellationToken)
        {
            if (request.ID == null)
                throw new NotFoundException(nameof(Course), request.ID);

            var course = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .ProjectTo<CourseDetailVM>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.CourseID == request.ID, cancellationToken);

            if(course == null)
                throw new NotFoundException(nameof(Course), request.ID);

            return course;
        }
    }
}
