using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AbpUsers", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.Name)
                   .HasColumnName("Name")
                   .IsRequired(true);

            builder.Property(x => x.Surname)
                   .HasColumnName("Surname")
                   .IsRequired(true);

            builder.Property(x => x.UserName)
                   .HasColumnName("UserName")
                   .IsRequired(true);

            builder .Property(x => x.NormalizedUserName)
                   .HasColumnName("NormalizedUserName")
                   .IsRequired(true);

            builder.Property(x => x.Email)
                   .HasColumnName("EmailAddress")
                   .IsRequired(true);

            builder.Property(x => x.EmailConfirmed)
                  .HasColumnName("IsEmailConfirmed")
                  .IsRequired(true);

            builder.Property(x => x.NormalizedEmail)
                  .HasColumnName("NormalizedEmailAddress")
                  .IsRequired(true);

            builder.Property(x => x.TenantId)
                   .HasColumnName("TenantId")
                   .IsRequired(false);

            builder.Property(x => x.PhoneNumber)
                   .HasColumnName("PhoneNumber")
                   .IsRequired(false);

            builder.Property(x => x.PhoneNumberConfirmed)
                   .HasColumnName("IsPhoneNumberConfirmed")
                   .IsRequired(true);

            builder.Property(x => x.IsActive)
                   .HasColumnName("IsActive")
                   .IsRequired(true);

            builder.Property(x => x.PasswordHash)
                   .HasColumnName("Password")
                   .IsRequired(true);

            builder.Property(x => x.SecurityStamp)
                   .HasColumnName("SecurityStamp")
                   .IsRequired(false);

            builder.Property(x => x.TwoFactorEnabled)
                   .HasColumnName("IsTwoFactorEnabled")
                   .IsRequired(true);

            builder.Property(x => x.LockoutEnabled)
                   .HasColumnName("IsLockoutEnabled")
                   .IsRequired(true);

            builder.Property(x => x.LockoutEnd)
                   .HasColumnName("LockoutEndDateUtc")
                   .IsRequired(false);

            builder.Property(x => x.AccessFailedCount)
                   .HasColumnName("AccessFailedCount")
                   .IsRequired(true);

            builder.Property(x => x.ConcurrencyStamp)
                   .HasColumnName("ConcurrencyStamp")
                   .IsRequired(false);
        }
    }
}
