using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Commands.UpdateCourse
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand>
    {
        private readonly ISchoolContext _context;

        public UpdateCourseCommandHandler(ISchoolContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseID == null)
                throw new NotFoundException(nameof(Course), request.CourseID);

            var courseToUpdate = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == request.CourseID);

            courseToUpdate.Title = request.Title;
            courseToUpdate.Credits = request.Credits;
            courseToUpdate.DepartmentID = request.DepartmentID;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
