using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Instructors.Commands.CreateInstructor
{
    public class CreateInstructorCommandHandler : IRequestHandler<CreateInstructorCommand>
    {
        private readonly ISchoolContext _context;

        public CreateInstructorCommandHandler(ISchoolContext context)
        {
            _context = context;
        }

        public async Task Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
        {
            _context.Instructors.Add(new Instructor
            {
                FirstMidName = request.FirstName,
                LastName = request.LastName,
                HireDate = request.HireDate
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
