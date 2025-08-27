using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class ConsumerTipoMap : IEntityTypeConfiguration<ConsumerTipo>
    {
        public void Configure(EntityTypeBuilder<ConsumerTipo> builder)
        {
            builder.ToTable("ConsumerTipos", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            //builder.Property(x => x.Tipo)
            //       .HasColumnName("Tipo")
            //       .IsRequired(true);

            builder.Property(x => x.CustomerId)
                   .HasColumnName("CustomerId")
                   .IsRequired(true);
        }
    }
}
