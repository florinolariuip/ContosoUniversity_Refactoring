using MediatR;

namespace Application.Students.Queries.DeleteConfirmation
{
    public class GetDeleteConfirmationQuery : IRequest<DeleteStudentVM>
    {
        public int? ID { get; set; }

        public GetDeleteConfirmationQuery(int? id)
        {
            ID = id;
        }
    }
}
