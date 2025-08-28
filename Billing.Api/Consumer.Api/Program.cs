using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using MoveEnergia.Billing.IoC;
using Newtonsoft.Json;
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

            builder.Services.AddGeneralValidationConfiguration(builder.Configuration);

            builder.Services.AddEntityFrameworkConfiguration(builder.Configuration);
            builder.Services.AddAddIdentityConfiguration(builder.Configuration);

            builder.Services.AddGeneralRepositoryConfiguration(builder.Configuration);
            builder.Services.AddGeneralAdapterConfiguration(builder.Configuration);

            builder.Services.AddGeneralServiceConfiguration(builder.Configuration);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            var app = builder.Build();

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

            app.Run();
        }
    }
}
