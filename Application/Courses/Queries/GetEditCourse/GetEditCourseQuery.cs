using MediatR;

namespace Application.Courses.Queries.GetEditCourse
{
    public class GetEditCourseQuery : IRequest<EditCourseVM>
    {
        public int? ID { get; set; }

        public GetEditCourseQuery(int? id)
        {
            ID = id;
        }
    }
}
