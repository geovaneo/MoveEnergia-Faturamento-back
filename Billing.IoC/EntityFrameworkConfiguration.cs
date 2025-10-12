using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Data.Context;
using Serilog;


namespace MoveEnergia.Billing.IoC
{
    public static class EntityFrameworkConfiguration
    {

        public static void AddEntityFrameworkConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            /*var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var builderConfiguration = builder.Build();*/

            

           /* using (var scope = services.CreateScope())
            {
                Log.Debug(">>>>> MIGRATIONS");
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
            }*/

        }
    }
}
//Script-Migration -From "20251011001722_AddDealsAddressFields" -To "20251011022807_AddDealsAddressFields2" -Context ApplicationDbContext