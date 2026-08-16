using MediatR;

namespace Application.Departments.Queries.GetUpdateDepartment
{
    public class GetUpdateDepartmentQuery : IRequest<UpdateDepartmentVM>
    {
        public int? ID { get; set; }

        public GetUpdateDepartmentQuery(int?  id)
        {
            ID = id;
        }
    }
}
