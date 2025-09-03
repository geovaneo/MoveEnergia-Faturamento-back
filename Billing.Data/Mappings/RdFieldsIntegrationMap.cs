using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class RdFieldsIntegrationMap : IEntityTypeConfiguration<RdFieldsIntegration>
    {
        public void Configure(EntityTypeBuilder<RdFieldsIntegration> builder)
        {
            builder.ToTable("RdFieldsIntegration", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.IdRd)
                   .HasColumnName("IdRd")
                   .IsRequired(true);

            builder.Property(x => x.Label)
                   .HasColumnName("Label")
                   .IsRequired(true);

            builder.Property(x => x.DataType)
                   .HasColumnName("DataType")
                   .IsRequired(true);

            builder.Property(x => x.Callback)
                   .HasColumnName("Callback")
                   .IsRequired(true);

            builder.Property(x => x.OrigemRD)
                   .HasColumnName("OrigemRD")
                   .IsRequired(true);

            builder.Property(x => x.Required)
                   .HasColumnName("Required")
                   .IsRequired(true);
        }
    }
}
