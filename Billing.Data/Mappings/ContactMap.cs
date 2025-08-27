using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class ContactMap : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.CustomerId)
                   .HasColumnName("CustomerId")
                   .IsRequired(true);

            builder.Property(x => x.TenantId)
                   .HasColumnName("TenantId")
                   .IsRequired(true);

            builder.Property(x => x.Nome)
                    .HasColumnName("Nome")
                    .IsRequired(false);

            builder.Property(x => x.PositionId)
                   .HasColumnName("PositionId")
                   .IsRequired(true);
        }
    }
}
