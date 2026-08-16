using MediatR;

namespace Application.Instructors.Queries.GetUpdateInstructor
{
    public class GetUpdateInstructorQuery : IRequest<UpdateInstructorVM>
    {
        public int? ID { get; set; }

        public GetUpdateInstructorQuery(int? id)
        {
            ID = id;
        }
    }
}
