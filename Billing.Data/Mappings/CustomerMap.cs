using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Enum;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.Name)
                   .HasColumnName("Name")
                   .IsRequired(false);

            builder.Property(x => x.RazoSocial)
                   .HasColumnName("RazoSocial")
                   .IsRequired(false);

            builder.Property(x => x.TipoCustomer)
                   .HasColumnName("TipoCustomer")
                   .IsRequired(true);

            builder.Property(x => x.Code)
                   .HasColumnName("Code")
                   .IsRequired(false);

            builder.Property(x => x.CreationTime)
                   .HasColumnName("CreationTime")
                   .IsRequired(true);

            builder.Property(x => x.TenantId)
                   .HasColumnName("TenantId")
                   .IsRequired(true);

            builder.Property(x => x.Mercado)
                   .HasColumnName("Mercado")
                   .IsRequired(true);

            builder.Property(x => x.Mercado)
                   .HasColumnName("Mercado")
                   .IsRequired(true);

            builder.Property(x => x.UserId)
                   .HasColumnName("UserId")
                   .IsRequired(true);

            builder.Property(x => x.UserId)
                   .HasColumnName("UserId")
                   .IsRequired(true);

            builder.Property(x => x.PathKey)
                   .HasColumnName("PathKey")
                   .IsRequired(false);

            builder.Property(x => x.CreatorUserId)
                   .HasColumnName("CreatorUserId")
                   .IsRequired(false);

            builder.Property(x => x.DeleterUserId)
                   .HasColumnName("DeleterUserId")
                   .IsRequired(false);

            builder.Property(x => x.DeletionTime)
                   .HasColumnName("DeletionTime")
                   .IsRequired(false);

            builder.Property(x => x.IsDeleted)
                   .HasColumnName("IsDeleted")
                   .IsRequired(true);

            builder.Property(x => x.IsDeleted)
                   .HasColumnName("IsDeleted")
                   .IsRequired(true);

            builder.Property(x => x.LastModificationTime)
                   .HasColumnName("LastModificationTime")
                   .IsRequired(false);

            builder.Property(x => x.LastModifierUserId)
                   .HasColumnName("LastModifierUserId")
                   .IsRequired(false);

            builder.HasMany(e => e.Addresses)
                   .WithOne(e => e.Customer)
                   .HasForeignKey(cu => cu.CustomerId)
                   .HasPrincipalKey(s => s.Id);

            builder.HasOne(c => c.User)
                   .WithOne(u => u.Customer)
                   .HasForeignKey<Customer>(c => c.UserId) 
                   .HasPrincipalKey<User>(u => u.Id);

        }
    }
}
