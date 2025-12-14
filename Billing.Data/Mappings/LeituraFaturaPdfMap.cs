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
                   .IsRequired(false);

            builder.Property(x => x.MesReferencia)
                   .HasColumnName("mesref")
                   .IsRequired(false);

            builder.Property(x => x.Vencimento)
                   .HasColumnName("vencimento")
                   .IsRequired(false);

            builder.Property(x => x.DataEmissao)
                   .HasColumnName("emissao")
                   .IsRequired(false);

            builder.Property(x => x.Valor)
                   .HasColumnName("Valor")
                   .IsRequired(false);

            builder.Property(x => x.NomeDistribuidora)
                   .HasColumnName("nomedistr")
                   .IsRequired(false);

            builder.Property(x => x.FileName)
                   .HasColumnName("filename")
                   .IsRequired(false);

            builder.Property(x => x.FileMD5)
                   .HasColumnName("filemd5")
                   .IsRequired(false);

            builder.Property(x => x.FolderName)
                   .HasColumnName("folder")
                   .IsRequired(false);

            builder.Property(x => x.LeituraAnterior)
                   .HasColumnName("leianterior")
                   .IsRequired(false);

            builder.Property(x => x.LeituraAtual)
                   .HasColumnName("leiatual")
                   .IsRequired(false);

            builder.Property(x => x.EnergiaConsumida)
                   .HasColumnName("energiaconsumida")
                   .IsRequired(false);

            builder.Property(x => x.EnergiaCompensada)
                   .HasColumnName("energiacompensada")
                   .IsRequired(false);

            builder.Property(x => x.EnergiaSaldo)
                   .HasColumnName("energiasaldo")
                   .IsRequired(false);

            builder.Property(x => x.CodBarras)
                   .HasColumnName("codbarras")
                   .IsRequired(false);

            builder.HasMany(c => c.Linhas)
                    .WithOne(a => a.FaturaPDF)
                    .HasForeignKey(a => a.FaturaPDFId);
        }
    }
}
