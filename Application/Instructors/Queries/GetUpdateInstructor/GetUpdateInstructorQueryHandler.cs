using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Instructors.Queries.GetUpdateInstructor
{
    public class GetUpdateInstructorQueryHandler : IRequestHandler<GetUpdateInstructorQuery, UpdateInstructorVM>
    {
        private readonly ISchoolContext _context;
        private readonly IMapper _mapper;

        public GetUpdateInstructorQueryHandler(ISchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UpdateInstructorVM> Handle(GetUpdateInstructorQuery request, CancellationToken cancellationToken)
        {
            if (request.ID == null)
                throw new NotFoundException(nameof(Instructor), request.ID);

            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments).ThenInclude(i => i.Course)
                .AsNoTracking()
                .ProjectTo<UpdateInstructorVM>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.InstructorID == request.ID, cancellationToken);

            if (instructor == null)
                throw new NotFoundException(nameof(Instructor), request.ID);

            return instructor;
        }
    }
}
