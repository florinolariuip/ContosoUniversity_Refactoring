using MediatR;

namespace Application.Students.Queries.GetUpdateStudent
{
    public class GetUpdateStudentQuery : IRequest<UpdateStudentVM>
    {
        public int? ID { get; set; }

        public GetUpdateStudentQuery(int? id)
        {
            ID = id;
        }
    }
}
