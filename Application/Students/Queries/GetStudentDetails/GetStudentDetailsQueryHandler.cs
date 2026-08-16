using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudentDetails
{
    public class GetStudentDetailsQueryHandler : IRequestHandler<GetStudentDetailsQuery, StudentDetailsVM>
    {
        private readonly ISchoolContext _context;
        private readonly IMapper _mapper;

        public GetStudentDetailsQueryHandler(ISchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StudentDetailsVM> Handle(GetStudentDetailsQuery request, CancellationToken cancellationToken)
        {
            if (request.ID == null)
                throw new NotFoundException(nameof(Student), request.ID);

            var student = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .AsNoTracking()
                .ProjectTo<StudentDetailsVM>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.StudentID == request.ID);

            if (student == null)
                throw new NotFoundException(nameof(Student), request.ID);

            return student;
        }
    }
}
