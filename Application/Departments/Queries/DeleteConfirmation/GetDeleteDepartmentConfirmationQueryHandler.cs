using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Departments.Queries.DeleteConfirmation
{
    public class GetDeleteDepartmentConfirmationQueryHandler : IRequestHandler<GetDeleteDepartmentConfirmationQuery, DeleteDepartmentVM>
    {
        private readonly ISchoolContext _context;
        private readonly IMapper _mapper;

        public GetDeleteDepartmentConfirmationQueryHandler(ISchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DeleteDepartmentVM> Handle(GetDeleteDepartmentConfirmationQuery request, CancellationToken cancellationToken)
        {
            if (request.ID == null)
                throw new NotFoundException(nameof(Department), request.ID);

            var department = await _context.Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .ProjectTo<DeleteDepartmentVM>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.DepartmentID == request.ID, cancellationToken);

            if (department == null)
                throw new NotFoundException(nameof(Department), request.ID);

            return department;
        }
    }
}
