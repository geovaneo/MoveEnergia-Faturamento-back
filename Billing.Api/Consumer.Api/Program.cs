using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MoveEnergia.Billing.Data.Context;
using MoveEnergia.Billing.IoC;
using Newtonsoft.Json;
using Serilog;
using System.Globalization;
using System.Reflection;

namespace MoveEnergia.Billing.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cultureInfo = new CultureInfo("pt-BR");


            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddLogging();

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.Indented;
                });


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MoveEnergia Billing API",
                    Version = "v1"
                });
            });

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(new[]
                {
                    Assembly.GetExecutingAssembly(),
                    Assembly.Load("MoveEnergia.Billing.Api"),
                    Assembly.Load("MoveEnergia.Billing.Core")
                });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            builder.Services.AddHttpClient();

            builder.Services.AddGeneralValidationConfiguration(builder.Configuration);

            builder.Services.AddEntityFrameworkConfiguration(builder.Configuration);
            builder.Services.AddAddIdentityConfiguration(builder.Configuration);

            builder.Services.AddGeneralRepositoryConfiguration(builder.Configuration);

            builder.Services.AddGeneralAdapterConfiguration(builder.Configuration);

            builder.Services.AddRdStationIntegrationAdapterConfiguration(builder.Configuration);
            builder.Services.AddRdStationIntegrationServiceConfiguration(builder.Configuration);
            

            builder.Services.AddGeneralServiceConfiguration(builder.Configuration);
            builder.Services.AddAdapterSettings(builder.Configuration);

            builder.Host.UseSerilog();

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            var builderConfiguration = configBuilder.Build();

            Log.Debug("LOADING DB CONTEXT COTESA");

            var connectionString = builderConfiguration.GetConnectionString("cotesa");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString)
                       .EnableSensitiveDataLogging()
                       .LogTo(Console.WriteLine, LogLevel.Debug)
                       .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
                       );
            Log.Debug("LOADING DB CONTEXT COTESA - DONE");

            var app = builder.Build();

            string envLogger = String.IsNullOrEmpty(env) ? "" : "."+env;
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                    .AddJsonFile($"appsettings{envLogger}.json")
                    .Build())
                .CreateLogger();

            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                    if (exception is ValidationException validationEx)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        var errors = validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                        await context.Response.WriteAsJsonAsync(errors);
                    }
                });
            });

            app.UseCors("CorsPolicy");
            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(options =>
                {
                    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MoveEnergia Billing API v1");
                });
            }
            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            ApplyMigrations(app);

            app.Run();
        }

        private static void ApplyMigrations(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Check and apply pending migrations
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    Console.WriteLine("Applying pending migrations...");
                    dbContext.Database.Migrate();
                    Console.WriteLine("Migrations applied successfully.");
                }
                else
                {
                    Console.WriteLine("No pending migrations found.");
                }
            }
        }

    }
}
