using MediatR;

namespace Application.Instructors.Queries.GetInstructorsLookup
{
    public class GetInstructorLookupQuery : IRequest<List<InstructorLookupVM>>
    {
    }
}
