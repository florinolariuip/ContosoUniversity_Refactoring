using Application.Common.Interfaces;
using MediatR;

namespace Application.System.Commands.SeedData
{
    public class SeedDataCommand : IRequest
    {
    }

    public class SeedDataCommandHandler : IRequestHandler<SeedDataCommand>
    {
        private readonly ISchoolContext _schoolContext;

        public SeedDataCommandHandler(ISchoolContext schoolContext)
        {
            _schoolContext = schoolContext;
        }

        public async Task Handle(SeedDataCommand request, CancellationToken cancellationToken)
        {
            var seeder = new DataSeeder(_schoolContext);
            await seeder.Seed();
        }
    }
}
