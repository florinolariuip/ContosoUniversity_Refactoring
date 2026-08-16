using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Students.Commands.DeleteStudent
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand>
    {
        private readonly ISchoolContext _context;

        public DeleteStudentCommandHandler(ISchoolContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _context.Students.FindAsync(request.ID);

            if (student == null)
                throw new NotFoundException(nameof(Student), request.ID);

            _context.Students.Remove(student);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
