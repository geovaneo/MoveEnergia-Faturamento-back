using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class FaturaCacheMap : IEntityTypeConfiguration<FaturaCache>
    {
        public void Configure(EntityTypeBuilder<FaturaCache> builder)
        {
            builder.ToTable("FaturaCache", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.FaturaId)
                   .HasColumnName("FaturaId")
                   .IsRequired(true);

            builder.Property(x => x.EnviadoWhatsapp)
                   .HasColumnName("EnviadoWhatsapp")
                   .IsRequired(true);

            builder.Property(x => x.DataEnviadoWhatsapp)
                   .HasColumnName("DataEnviadoWhatsapp")
                   .IsRequired(true);

            builder.Property(x => x.MesReferencia)
                   .HasColumnName("MesReferencia")
                   .IsRequired(true);

            builder.Property(x => x.Status)
                   .HasColumnName("Status")
                   .IsRequired(false);

            builder.Property(x => x.NumeroEnviado)
                   .HasColumnName("NumeroEnviado")
                   .IsRequired(false);

            builder.Property(x => x.Tentativa)
                   .HasColumnName("Tentativa")
                   .IsRequired(true);

            builder.Property(x => x.UrlBoleto)
                   .HasColumnName("UrlBoleto")
                   .IsRequired(false);

            builder.Property(x => x.SubOrigem)
                   .HasColumnName("SubOrigem")
                   .IsRequired(false);

            builder.Property(x => x.Token)
                   .HasColumnName("Token")
                   .IsRequired(false);

            builder.Property(x => x.StatusEnviado)
                   .HasColumnName("StatusEnviado")
                   .IsRequired(false);

            builder.Property(x => x.Hash)
                   .HasColumnName("Hash")
                   .IsRequired(false);
        }
    }
}
