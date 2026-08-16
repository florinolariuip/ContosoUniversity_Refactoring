using Application.System.Commands.SeedData;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace Web
{
    public class Program
    {
        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var env = services.GetRequiredService<IWebHostEnvironment>();
                    // rulează seed doar dacă NU e environment "Testing"
                    if (!env.IsEnvironment("Testing"))
                    {
                        var context = services.GetRequiredService<Infrastructure.SchoolContext>();

                        context.Database.EnsureCreated();

                        var mediator = services.GetRequiredService<IMediator>();
                        await mediator.Send(new SeedDataCommand(), CancellationToken.None);
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
