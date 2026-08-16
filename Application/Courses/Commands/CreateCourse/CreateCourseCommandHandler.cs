using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Courses.Commands.CreateCourse
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand>
    {
        private readonly ISchoolContext _context;

        public CreateCourseCommandHandler(ISchoolContext context)
        {
            _context = context;
        }

        public async Task Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = new Course
            {
                Id = request.CourseID,
                Title = request.Title,
                Credits = request.Credits,
                DepartmentID = request.DepartmentID
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
