using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Students.Commands.CreateStudent
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand>
    {
        private readonly ISchoolContext _context;

        public CreateStudentCommandHandler(ISchoolContext context)
        {
            _context = context;
        }

        public async Task Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            _context.Students.Add(new Student 
            {
                FirstMidName = request.FirstName,
                LastName = request.LastName,
                EnrollmentDate = request.EnrollmentDate
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
