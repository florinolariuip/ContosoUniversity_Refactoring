using MediatR;

namespace Application.Departments.Queries.DeleteConfirmation
{
    public class GetDeleteDepartmentConfirmationQuery : IRequest<DeleteDepartmentVM>
    {
        public int? ID { get; set; }

        public GetDeleteDepartmentConfirmationQuery(int? id)
        {
            ID = id;
        }
    }
}
