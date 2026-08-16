using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Web.IntegrationTests.ApiTests
{
    public class CoursesApiTests : IClassFixture<WebApplicationFactory<Web.Startup>>
    {
        private readonly WebApplicationFactory<Web.Startup> _factory;

        public CoursesApiTests(WebApplicationFactory<Web.Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                // setează environment = Testing pentru a evita seed-ul din Program.Main
                builder.UseSetting("environment", "Testing");

                builder.ConfigureServices(services =>
                {
                    // înlocuiește DbContext Options pentru Infrastructure.SchoolContext cu InMemory
                    var descriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<Infrastructure.SchoolContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<Infrastructure.SchoolContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });

                    // Optional: poți popula aici DB-ul de test cu date minimale
                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope())
                    {
                        var ctx = scope.ServiceProvider.GetRequiredService<Infrastructure.SchoolContext>();
                        ctx.Database.EnsureCreated();
                        // Adaugă seed de test dacă ai nevoie
                    }
                });
            });
        }

        [Fact]
        public async Task Get_Courses_ReturnsOk()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/courses");
            response.EnsureSuccessStatusCode(); // asigură 200-299
        }
    }
}

