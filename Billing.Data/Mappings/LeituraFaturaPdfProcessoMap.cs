using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class LeituraFaturaPdfProcessoMap : IEntityTypeConfiguration<LeituraFaturaPdfProcesso>
    {
        public void Configure(EntityTypeBuilder<LeituraFaturaPdfProcesso> builder)
        {
            builder.ToTable("LeituraFaturaPdfProcesso", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Inicio)
                   .HasColumnName("inicio")
                   .IsRequired(false);

            builder.Property(x => x.Termino)
                   .HasColumnName("termino")
                   .IsRequired(false);

        }
    }
}
