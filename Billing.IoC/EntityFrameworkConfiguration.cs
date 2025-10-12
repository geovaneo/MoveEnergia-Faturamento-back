using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Data.Context;


namespace MoveEnergia.Billing.IoC
{
    public static class EntityFrameworkConfiguration
    {

        public static void AddEntityFrameworkConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var builderConfiguration = builder.Build();

            var connectionString = builderConfiguration.GetConnectionString("cotesa");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString)
                       .EnableSensitiveDataLogging()
                       .LogTo(Console.WriteLine, LogLevel.Information)
                       .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
                       );

            
        }
    }
}
