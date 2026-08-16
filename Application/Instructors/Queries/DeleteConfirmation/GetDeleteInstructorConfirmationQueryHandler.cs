using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Instructors.Queries.DeleteConfirmation
{
    public class GetDeleteInstructorConfirmationQueryHandler : IRequestHandler<GetDeleteInstructorConfirmationQuery, DeleteInstructorVM>
    {
        private readonly ISchoolContext _context;
        private readonly IMapper _mapper;

        public GetDeleteInstructorConfirmationQueryHandler(ISchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DeleteInstructorVM> Handle(GetDeleteInstructorConfirmationQuery request, CancellationToken cancellationToken)
        {
            if (request.ID == null)
                throw new NotFoundException(nameof(Instructor), request.ID);

            var instructor = await _context.Instructors
                .AsNoTracking()
                .ProjectTo<DeleteInstructorVM>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.InstructorID == request.ID, cancellationToken);

            if (instructor == null)
                throw new NotFoundException(nameof(Instructor), request.ID);

            return instructor;
        }
    }
}
