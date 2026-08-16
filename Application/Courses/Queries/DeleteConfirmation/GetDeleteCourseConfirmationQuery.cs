using MediatR;

namespace Application.Courses.Queries.DeleteConfirmation
{
    public class GetDeleteCourseConfirmationQuery : IRequest<DeleteCourseVM>
    {
        public int? ID { get; set; }

        public GetDeleteCourseConfirmationQuery(int? id)
        {
            ID = id;
        }
    }
}
