using MediatR;
using System.Collections.Generic;

namespace Application.Courses.Queries.GetCourseList
{
    public class GetCourseListQuery : IRequest<List<CourseSelectionVM>>
    {
        public int? InstructorID { get; set; }

        public GetCourseListQuery(int? instructorID)
        {
            InstructorID = instructorID;
        }
    }
}
