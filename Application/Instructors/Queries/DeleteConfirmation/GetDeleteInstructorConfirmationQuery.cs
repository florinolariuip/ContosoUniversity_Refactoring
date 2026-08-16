using MediatR;

namespace Application.Instructors.Queries.DeleteConfirmation
{
    public class GetDeleteInstructorConfirmationQuery : IRequest<DeleteInstructorVM>
    {
        public int? ID { get; set; }

        public GetDeleteInstructorConfirmationQuery(int? id)
        {
            ID = id;
        }
    }
}
