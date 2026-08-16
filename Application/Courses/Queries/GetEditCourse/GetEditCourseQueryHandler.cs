using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Queries.GetEditCourse
{
    public class GetEditCourseQueryHandler : IRequestHandler<GetEditCourseQuery, EditCourseVM>
    {
        private readonly ISchoolContext _context;
        private readonly IMapper _mapper;

        public GetEditCourseQueryHandler(ISchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EditCourseVM> Handle(GetEditCourseQuery request, CancellationToken cancellationToken)
        {
            if (request.ID == null)
                throw new NotFoundException(nameof(Course), request.ID);

            var course = await _context.Courses
                .AsNoTracking()
                .ProjectTo<EditCourseVM>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.CourseID == request.ID, cancellationToken);

            if (course == null)
                throw new NotFoundException(nameof(Course), request.ID);

            return course;
        }
    }
}
