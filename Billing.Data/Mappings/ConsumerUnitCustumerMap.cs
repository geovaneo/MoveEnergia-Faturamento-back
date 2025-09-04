using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class ConsumerUnitCustumerMap : IEntityTypeConfiguration<ConsumerUnitCustumer>
    {
        public void Configure(EntityTypeBuilder<ConsumerUnitCustumer> builder)
        {
            builder.ToTable("ConsumerUnitCustumer", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.CustomerId)
                   .HasColumnName("CustomerId")
                   .IsRequired(true);

            builder.Property(x => x.UC)
                   .HasColumnName("UC")
                   .IsRequired(true);

            builder.Property(x => x.CreateDate)
                   .HasColumnName("CreateDate")
                   .IsRequired(true);

        }
    }
}
