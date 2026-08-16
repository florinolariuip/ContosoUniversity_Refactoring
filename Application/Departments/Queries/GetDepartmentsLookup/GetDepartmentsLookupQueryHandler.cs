using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Interfaces;

namespace Application.Departments.Queries.GetDepartmentsLookup
{
    public class GetDepartmentsLookupQueryHandler : IRequestHandler<GetDepartmentsLookupQuery, List<DepartmentLookupVM>>
    {
        private readonly ISchoolContext _context;
        private readonly IMapper _mapper;

        public GetDepartmentsLookupQueryHandler(ISchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DepartmentLookupVM>> Handle(GetDepartmentsLookupQuery request, CancellationToken cancellationToken)
        {
            return await _context.Departments
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ProjectTo<DepartmentLookupVM>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
