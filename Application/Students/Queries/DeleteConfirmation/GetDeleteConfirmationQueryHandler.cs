using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.DeleteConfirmation
{
    public class GetDeleteConfirmationQueryHandler : IRequestHandler<GetDeleteConfirmationQuery, DeleteStudentVM>
    {
        private readonly ISchoolContext _context;
        private readonly IMapper _mapper;

        public GetDeleteConfirmationQueryHandler(ISchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DeleteStudentVM> Handle(GetDeleteConfirmationQuery request, CancellationToken cancellationToken)
        {
            if (request.ID == null)
                throw new NotFoundException(nameof(Student), request.ID);

            var student = await _context.Students
                .AsNoTracking()
                .ProjectTo<DeleteStudentVM>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.StudentID == request.ID);

            if (student == null)
                throw new NotFoundException(nameof(Student), request.ID);

            return student;
        }
    }
}
