using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class DistributorMap : IEntityTypeConfiguration<Distributor>
    {
        public void Configure(EntityTypeBuilder<Distributor> builder)
        {
            builder.ToTable("Distributor", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.Nome)
                   .HasColumnName("Nome")
                   .IsRequired(false);

            builder.Property(x => x.Cnpj)
                   .HasColumnName("Cnpj")
                   .IsRequired(false);

            builder.Property(x => x.Sigla)
                   .HasColumnName("Sigla")
                   .IsRequired(false);

            builder.Property(x => x.UF)
                   .HasColumnName("UF")
                   .IsRequired(false);

            builder.Property(x => x.ICMSTUSDc)
                   .HasColumnName("ICMSTUSDc")
                   .IsRequired(true);

            builder.Property(x => x.ICMSTE)
                   .HasColumnName("ICMSTE")
                   .IsRequired(true);

            builder.Property(x => x.ICMSComum)
                   .HasColumnName("ICMSComum")
                   .IsRequired(true);

            builder.Property(x => x.PISComum)
                   .HasColumnName("PISComum")
                   .IsRequired(true);

            builder.Property(x => x.COFINSComum)
                   .HasColumnName("COFINSComum")
                   .IsRequired(true);

            builder.Property(x => x.ICMSInjetada)
                   .HasColumnName("ICMSInjetada")
                   .IsRequired(true);

            builder.Property(x => x.PISInjetada)
                   .HasColumnName("PISInjetada")
                   .IsRequired(true);

            builder.Property(x => x.COFINSInjetada)
                   .HasColumnName("COFINSInjetada")
                   .IsRequired(true);

            builder.Property(x => x.DataReajustePrevisto)
                   .HasColumnName("DataReajustePrevisto")
                   .IsRequired(true);

            builder.Property(x => x.IdCooperativa)
                   .HasColumnName("ID_COOPERATIVA")
                   .IsRequired(false);

            builder.Property(x => x.IsActive)
                   .HasColumnName("IsActive")
                   .IsRequired(true);

            builder.Property(x => x.NomeDeApresentacao)
                   .HasColumnName("NomeDeApresentacao")
                   .IsRequired(false);
        }
    }
}
