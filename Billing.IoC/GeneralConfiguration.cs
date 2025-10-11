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
using MoveEnergia.Rdstation.Adapter.Configuration;
using MoveEnergia.Rdstation.Adapter.Interface.Service;
using MoveEnergia.Rdstation.Adapter.Service;
using MoveEnergia.RdStation.Adapter;
using MoveEnergia.RdStation.Adapter.Configuration;
using MoveEnergia.RdStation.Adapter.Interface.Adapter;
using MoveEnergia.RdStation.Adapter.Interface.Service;


namespace MoveEnergia.Billing.IoC
{
    public static class GeneralConfiguration
    {
        public static void AddGeneralRepositoryConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDistributorRepository, DistributorRepository>();
            services.AddScoped<IRdFieldsIntegrationRepository, RdFieldsIntegrationRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IConsumerUnitCustumerRepository, ConsumerUnitCustumerRepository>();
            services.AddScoped<IDealRepository, DealRepository>();
            services.AddScoped<IConsumerUnitRepository, ConsumerUnitRepository>();            
        }

        public static void AddGeneralServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDistributorService, DistributorService>();
        }

        public static void AddRdStationIntegrationServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRdStationIntegrationService, RdStationIntegrationService>();
            services.AddScoped<IRdCargaService, RdCargaService>();
            services.AddScoped<IHttpService, HttpService>();
        }
        public static void AddGeneralAdapterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDistributorAdapter, DistributorAdapter>();
            services.AddScoped<IRdstationIntegrationAdapter, RdstationIntegrationAdapter>();
        }
        public static void AddRdStationIntegrationAdapterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRdstationIntegrationAdapter, RdstationIntegrationAdapter>();
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

            services.AddMemoryCache();
        }
        public static void AddAdapterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RdStationConfiguration>(configuration.GetSection("RdStationConfiguration"));
            services.Configure<RdStationIntegrationCustomer>(configuration.GetSection("RdStationIntegrationCustomer"));

        }
    }
}
