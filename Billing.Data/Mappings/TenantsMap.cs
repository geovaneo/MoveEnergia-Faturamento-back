using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class TenantsMap : IEntityTypeConfiguration<Tenants>
    {
        public void Configure(EntityTypeBuilder<Tenants> builder)
        {
            builder.ToTable("AbpTenants", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id");

            builder.Property(x => x.CreationTime)
                   .HasColumnName("CreationTime");

            builder.Property(x => x.CreatorUserId)
                   .HasColumnName("CreatorUserId");

            builder.Property(x => x.LastModificationTime)
                   .HasColumnName("LastModificationTime");

            builder.Property(x => x.LastModifierUserId)
                   .HasColumnName("LastModifierUserId");

            builder.Property(x => x.IsDeleted)
                   .HasColumnName("IsDeleted");

            builder.Property(x => x.DeleterUserId)
                   .HasColumnName("DeleterUserId");

            builder.Property(x => x.DeletionTime)
                   .HasColumnName("DeletionTime");

            builder.Property(x => x.TenancyName)
                   .HasColumnName("TenancyName");

            builder.Property(x => x.Name)
                   .HasColumnName("Name");

            builder.Property(x => x.ConnectionString)
                   .HasColumnName("ConnectionString");

            builder.Property(x => x.IsActive)
                   .HasColumnName("IsActive");

            builder.Property(x => x.EditionId)
                   .HasColumnName("EditionId");

            builder.Property(x => x.RdStationSubOrigem)
                   .HasColumnName("RdStationSubOrigem");

            builder.Property(x => x.RdStationToken)
                   .HasColumnName("RdStationToken");
        }
    }
}
