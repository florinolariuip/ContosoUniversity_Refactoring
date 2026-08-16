using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CourseAssignment.Commands
{
    public class UpdateCourseAssignmentsCommandHandler : IRequestHandler<UpdateCourseAssignmentsCommand>
    {
        private readonly ISchoolContext _context;

        public UpdateCourseAssignmentsCommandHandler(ISchoolContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateCourseAssignmentsCommand request, CancellationToken cancellationToken)
        {
            if (request.InstructorID == null)
                throw new NotFoundException(nameof(Instructor), request.InstructorID);

            var instructor = await _context.Instructors.Include(x => x.CourseAssignments).Where(x => x.Id == request.InstructorID).FirstOrDefaultAsync();

            if(instructor == null)
                throw new NotFoundException(nameof(Instructor), request.InstructorID);

            if (request.SelectedCourses == null)
            {
                instructor.CourseAssignments = new List<Domain.Entities.CourseAssignment>();
            }

            var selectedCoursesHS = new HashSet<string>(request.SelectedCourses);
            var instructorCourses = new HashSet<int>
                (instructor.CourseAssignments.Select(c => c.CourseID));

            foreach (var course in _context.Courses)
            {
                if (selectedCoursesHS.Contains(course.Id.ToString()))
                {
                    if (!instructorCourses.Contains(course.Id))
                    {
                        instructor.CourseAssignments.Add(new Domain.Entities.CourseAssignment { InstructorID = instructor.Id, CourseID = course.Id });
                    }
                }
                else
                {

                    if (instructorCourses.Contains(course.Id))
                    {
                        Domain.Entities.CourseAssignment courseToRemove = instructor.CourseAssignments.FirstOrDefault(i => i.CourseID == course.Id);
                        _context.CourseAssignments.Remove(courseToRemove);
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
