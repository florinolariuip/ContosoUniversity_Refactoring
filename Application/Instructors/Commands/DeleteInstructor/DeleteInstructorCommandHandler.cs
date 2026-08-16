using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Instructors.Commands.DeleteInstructor
{
    public class DeleteInstructorCommandHandler : IRequestHandler<DeleteInstructorCommand>
    {
        private readonly ISchoolContext _context;

        public DeleteInstructorCommandHandler(ISchoolContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
        {
            var instructor = await _context.Instructors
                .Include(i => i.CourseAssignments)
                .SingleAsync(i => i.Id == request.ID);

            if (instructor == null)
                throw new NotFoundException(nameof(Instructor), request.ID);

            var departments = await _context.Departments
                .Where(d => d.InstructorID == request.ID)
                .ToListAsync();
            departments.ForEach(d => d.InstructorID = null);

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
