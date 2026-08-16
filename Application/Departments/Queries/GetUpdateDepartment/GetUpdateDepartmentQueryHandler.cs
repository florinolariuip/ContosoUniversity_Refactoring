using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Departments.Queries.GetUpdateDepartment
{
    public class GetUpdateDepartmentQueryHandler : IRequestHandler<GetUpdateDepartmentQuery, UpdateDepartmentVM>
    {
        private readonly ISchoolContext _context;
        private readonly IMapper _mapper;

        public GetUpdateDepartmentQueryHandler(ISchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UpdateDepartmentVM> Handle(GetUpdateDepartmentQuery request, CancellationToken cancellationToken)
        {
            if (request.ID == null)
                throw new NotFoundException(nameof(Department), request.ID);

            var department = await _context.Departments
                .Include(i => i.Administrator)
                .AsNoTracking()
                .ProjectTo<UpdateDepartmentVM>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.DepartmentID == request.ID, cancellationToken);

            if(department == null)
                throw new NotFoundException(nameof(Department), request.ID);

            return department;
        }
    }
}
