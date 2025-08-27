using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Data.Context;
using MoveEnergia.Billing.Adapter;
using MoveEnergia.Billing.Core.Interface.Adapter;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Core.Interface.Service;
using MoveEnergia.Billing.Data.Repository;
using MoveEnergia.Billing.Service;
using Service;

namespace MoveEnergia.Billing.IoC
{
    public static class GeneralConfiguration
    {
        public static void AddGeneralRepositoryConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITenantsRepository, TenantsRepository>();
            services.AddScoped<IConsumerUnitRepository, ConsumerUnitRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IConsumerUnitMeasurementRepository, ConsumerUnitMeasurementRepository>();
            services.AddScoped<IDetalhesFaturaCacheRepository, DetalhesFaturaCacheRepository>();
            services.AddScoped<IDistributorRepository, DistributorRepository>();
        }

        public static void AddGeneralServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITenantsService, TenantsService>();
            services.AddScoped<IConsumerUnitService, ConsumerUnitService>();
            services.AddScoped<IHomeInfoService, HomeInfoService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IDistributorService, DistributorService>();
        }

        public static void AddGeneralAdapterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IConsumerUnitAdapter, ConsumerUnitAdapter>();
            services.AddScoped<IAuthenticationAdapter, AuthenticationAdapter>();
            services.AddScoped<IHomeInfoAdapter, HomeInfoAdapter>();
            services.AddScoped<IDistributorAdapter, DistributorAdapter>();
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
