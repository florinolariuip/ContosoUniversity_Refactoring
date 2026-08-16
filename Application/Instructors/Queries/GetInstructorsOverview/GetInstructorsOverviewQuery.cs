using MediatR;

namespace Application.Instructors.Queries.GetInstructorsOverview
{
    public class GetInstructorsOverviewQuery : IRequest<InstructorsOverviewVM>
    {
        public int? SelectedInstructorID { get; set; }

        public int? SelectedCourseID { get; set; }

        public GetInstructorsOverviewQuery(int? selectedInstructorID, int? selectedCourseID)
        {
            SelectedInstructorID = selectedInstructorID;
            SelectedCourseID = selectedCourseID;
        }
    }
}
