using LibraryManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace LibraryManagementSystem.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //builder.ConfigureAppConfiguration((context, config) =>
            //{
            //    // Load the configuration for tests depending on the env
            //    var env = context.HostingEnvironment.EnvironmentName;

            //    config.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

            //    config.AddEnvironmentVariables();
            //});

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<LibraryDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var serviceProvider = services.BuildServiceProvider();
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                
                services.AddDbContext<LibraryDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("LibraryDb"))); 

            });
        }
    }
}
