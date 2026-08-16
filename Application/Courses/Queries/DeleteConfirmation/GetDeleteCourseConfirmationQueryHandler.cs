using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Queries.DeleteConfirmation
{
    public class GetDeleteCourseConfirmationQueryHandler : IRequestHandler<GetDeleteCourseConfirmationQuery, DeleteCourseVM>
    {
        private readonly ISchoolContext _context;
        private readonly IMapper _mapper;

        public GetDeleteCourseConfirmationQueryHandler(ISchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DeleteCourseVM> Handle(GetDeleteCourseConfirmationQuery request, CancellationToken cancellationToken)
        {
            if (request.ID == null)
                throw new NotFoundException(nameof(Course), request.ID);

            var course = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .ProjectTo<DeleteCourseVM>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.CourseID == request.ID, cancellationToken);

            if (course == null)
                throw new NotFoundException(nameof(Course), request.ID);

            return course;
        }
    }
}
