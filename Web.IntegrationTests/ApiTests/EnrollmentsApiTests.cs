using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Web.IntegrationTests.ApiTests
{
    public class EnrollmentsApiTests : IClassFixture<WebApplicationFactory<Web.Startup>>
    {
        private readonly WebApplicationFactory<Web.Startup> _factory;

        public EnrollmentsApiTests(WebApplicationFactory<Web.Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseSetting("environment", "Testing");

                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<Infrastructure.SchoolContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<Infrastructure.SchoolContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });

                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope())
                    {
                        var ctx = scope.ServiceProvider.GetRequiredService<Infrastructure.SchoolContext>();
                        ctx.Database.EnsureCreated();
                    }
                });
            });
        }

        [Fact]
        public async Task Get_Enrollments_ReturnsOk()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/enrollments");
            response.EnsureSuccessStatusCode();
        }
    }
}

