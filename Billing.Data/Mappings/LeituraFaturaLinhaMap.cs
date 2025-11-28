using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class LeituraFaturaLinhaMap : IEntityTypeConfiguration<LeituraFaturaLinha>
    {
        public void Configure(EntityTypeBuilder<LeituraFaturaLinha> builder)
        {
            builder.ToTable("LeituraFaturaLinha", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.Descricao)
                   .HasColumnName("Descricao")
                   .IsRequired(false);

            builder.Property(x => x.Unidade)
                   .HasColumnName("unidade")
                   .IsRequired(false);

            builder.Property(x => x.Qtd)
                   .HasColumnName("qtd")
                   .HasPrecision(12, 5)
                   .IsRequired(false);

            builder.Property(x => x.PrecoUnit)
                   .HasColumnName("precounit")
                   .HasPrecision(12, 8)
                   .IsRequired(false);

            builder.Property(x => x.Valor)
                   .HasColumnName("Valor")
                   .HasPrecision(10, 4)
                   .IsRequired(false);

            builder.Property(x => x.CofinsPIS)
                   .HasColumnName("cofinspis")
                   .HasPrecision(10, 4)
                   .IsRequired(false);

            builder.Property(x => x.ICMSBaseCalc)
                   .HasColumnName("icmsbase")
                   .HasPrecision(10, 4)
                   .IsRequired(false);

            builder.Property(x => x.ICMSAliq)
                   .HasColumnName("icmsaliq")
                   .HasPrecision(10, 4)
                   .IsRequired(false);

            builder.Property(x => x.ICMS)
                   .HasColumnName("icms")
                   .HasPrecision(10, 4)
                   .IsRequired(false);

            builder.Property(x => x.TarifaUnit)
                   .HasColumnName("tarifaunit")
                   .HasPrecision(12,8)
                   .IsRequired(false);

            builder.HasOne(e => e.FaturaPDF)
                   .WithMany(c => c.Linhas)
                   .HasForeignKey(e => e.FaturaPDFId);
        }
    }
}
