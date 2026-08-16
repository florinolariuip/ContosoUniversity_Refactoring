using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Instructors.Commands.UpdateInstructor
{
    public class UpdateInstructorCommandHandler : IRequestHandler<UpdateInstructorCommand>
    {
        private readonly ISchoolContext _context;

        public UpdateInstructorCommandHandler(ISchoolContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateInstructorCommand request, CancellationToken cancellationToken)
        {
            if (request.InstructorID == null)
                throw new NotFoundException(nameof(Instructor), request.InstructorID);

            var instructorToUpdate = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .FirstOrDefaultAsync(m => m.Id == request.InstructorID);

            if (instructorToUpdate == null)
                throw new NotFoundException(nameof(Instructor), request.InstructorID);

            instructorToUpdate.LastName = request.LastName;
            instructorToUpdate.FirstMidName = request.FirstName;
            instructorToUpdate.HireDate = request.HireDate;

            if (instructorToUpdate.OfficeAssignment == null)
                instructorToUpdate.OfficeAssignment = new OfficeAssignment();

            instructorToUpdate.OfficeAssignment.Location = request.OfficeLocation;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
