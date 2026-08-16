using MediatR;

namespace Application.Departments.Queries.GetDepartmentsLookup
{
    public class GetDepartmentsLookupQuery : IRequest<List<DepartmentLookupVM>>
    {
    }
}
