using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoveEnergia.Billing.Adapter;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Adapter;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Core.Interface.Service;
using MoveEnergia.Billing.Core.Validation;
using MoveEnergia.Billing.Data.Context;
using MoveEnergia.Billing.Data.Repository;
using MoveEnergia.Billing.Service;


namespace MoveEnergia.Billing.IoC
{
    public static class GeneralConfiguration
    {
        public static void AddGeneralRepositoryConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDistributorRepository, DistributorRepository>();
        }

        public static void AddGeneralServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDistributorService, DistributorService>();
        }

        public static void AddGeneralAdapterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDistributorAdapter, DistributorAdapter>();
        }

        public static void AddGeneralValidationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssemblyContaining<DistributorValidation>();
        }

        public static void AddAddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole<long>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void AddGeneralConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            //services.Configure<SoapSapConfiguration>(configuration.GetSection("SOAPSAP"));
            //services.Configure<OrdemManutencaoConfiguration>(configuration.GetSection("CargaOrdemManutencaoConfiguration"));
            //services.Configure<AnaliseRiscoConfiguration>(configuration.GetSection("AnaliseRiscoConfiguration"));

            services.AddMemoryCache();
        }

    }
}
