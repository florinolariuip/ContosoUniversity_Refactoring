using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.UpdateStudent
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand>
    {
        private readonly ISchoolContext _context;

        public UpdateStudentCommandHandler(ISchoolContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            if (request.StudentID == null)
                throw new NotFoundException(nameof(Student), request.StudentID);

            var studentToUpdate = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == request.StudentID);

            studentToUpdate.FirstMidName = request.FirstName;
            studentToUpdate.LastName = request.LastName;
            studentToUpdate.EnrollmentDate = request.EnrollmentDate;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
