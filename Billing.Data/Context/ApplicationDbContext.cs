using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        public DbSet<Tenants> Tenants { get; set; }
        public DbSet<ConsumerUnit> ConsumerUnits { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<DetalhesFaturaCache> DetalhesFaturaCache { get; set; }
        public DbSet<FaturaCache> FaturaCache { get; set; }
        public DbSet<RdFieldsIntegration> RdFieldsIntegration { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Deals> deals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken); 

                Console.WriteLine($"Erro: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                throw new Exception($"{ex.Message}. Inner Exception: {ex.InnerException?.Message}", ex);
            }
        }
    }
}
