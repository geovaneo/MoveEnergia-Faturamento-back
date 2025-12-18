using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class LeituraFaturaPdfMapLog : IEntityTypeConfiguration<LeituraFaturaPdfLog>
    {
        public void Configure(EntityTypeBuilder<LeituraFaturaPdfLog> builder)
        {
            builder.ToTable("LeituraFaturaPdfLog", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

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

            builder.Property(x => x.DataHora)
                   .HasColumnName("datahora")
                   .IsRequired(true);

            builder.Property(x => x.Mensagem)
                   .HasColumnName("mensagem")
                   .IsRequired(false);

            builder.HasOne(e => e.ProcessoEntity)
                   .WithMany(c => c.Logs)
                   .HasForeignKey(e => e.Processo);
        }
    }
}
