using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class LeituraFaturaPdfMap : IEntityTypeConfiguration<LeituraFaturaPdf>
    {
        public void Configure(EntityTypeBuilder<LeituraFaturaPdf> builder)
        {
            builder.ToTable("LeituraFaturaPdf", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.UC)
                   .HasColumnName("UC")
                   .IsRequired(true);

            builder.Property(x => x.MesReferencia)
                   .HasColumnName("mesref")
                   .IsRequired(true);

            builder.Property(x => x.Vencimento)
                   .HasColumnName("vencimento")
                   .IsRequired(false);

            builder.Property(x => x.Valor)
                   .HasColumnName("Valor")
                   .IsRequired(false);

        }
    }
}
