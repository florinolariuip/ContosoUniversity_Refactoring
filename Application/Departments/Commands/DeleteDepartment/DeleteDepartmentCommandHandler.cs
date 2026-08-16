using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand>
    {
        private readonly ISchoolContext _context;

        public DeleteDepartmentCommandHandler(ISchoolContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _context.Departments.FindAsync(request.ID);

            if (department == null)
                throw new NotFoundException(nameof(Department), request.ID);

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
