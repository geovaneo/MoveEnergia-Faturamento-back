using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;
using System.Reflection.Emit;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class ConsumerUnitMap : IEntityTypeConfiguration<ConsumerUnit>
    {
        public void Configure(EntityTypeBuilder<ConsumerUnit> builder)
        {
            builder.ToTable("ConsumerUnit", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id");

            builder.Property(x => x.Nome)
                   .HasColumnName("Nome")
                   .IsRequired(false);

            builder.Property(x => x.UC)
                   .HasColumnName("UC")
                   .IsRequired(false);

            builder.Property(x => x.Cnpj)
                   .HasColumnName("Cnpj")
                   .IsRequired(false);

            builder.Property(x => x.ExpectativaOperacao)
                   .HasColumnName("ExpectativaOperacao")
                   .IsRequired(false);

            builder.Property(x => x.DataMigracao)
                   .HasColumnName("DataMigracao")
                   .IsRequired(false);

            builder.Property(x => x.NomeUsina)
                   .HasColumnName("NomeUsina")
                   .IsRequired(false);

            builder.Property(x => x.FatorCapacidade)
                   .HasColumnName("FatorCapacidade")
                   .IsRequired(false);

            builder.Property(x => x.Fonte)
                   .HasColumnName("Fonte")
                   .IsRequired(false);

            builder.Property(x => x.ConnectionId)
                   .HasColumnName("ConnectionId")
                   .IsRequired(false);

            builder.Property(x => x.Enquadramento)
                   .HasColumnName("Enquadramento")
                   .IsRequired(false);

            builder.Property(x => x.PotenciakWCA)
                   .HasColumnName("PotenciakWCA")
                   .IsRequired(false);

            builder.Property(x => x.DemandaContradada)
                   .HasColumnName("DemandaContradada")
                   .IsRequired(false);

            builder.Property(x => x.UnidadeStatusId)
                   .HasColumnName("UnidadeStatusId")
                   .IsRequired(true);

            builder.Property(x => x.StateId)
                   .HasColumnName("StateId")
                   .IsRequired(true);

            builder.Property(x => x.SubgroupId)
                   .HasColumnName("SubgroupId")
                   .IsRequired(true);

            builder.Property(x => x.TariffModalityId)
                   .HasColumnName("Tariff_ModalityId")
                   .IsRequired(true);

            builder.Property(x => x.DistributorId)
                   .HasColumnName("DistributorId")
                   .IsRequired(true);

            builder.Property(x => x.TenantId)
                   .HasColumnName("TenantId")
                   .IsRequired(true);

            builder.Property(x => x.CustomerId)
                   .HasColumnName("CustomerId")
                   .IsRequired(true);

            //builder.Property(x => x.Tipo)
            //       .HasColumnName("Tipo")
            //       .IsRequired(true);

            builder.Property(x => x.CreationTime)
                   .HasColumnName("CreationTime")
                   .IsRequired(true);

            builder.Property(x => x.UserId)
                   .HasColumnName("UserId")
                   .IsRequired(true);

            builder.Property(x => x.DemandaContradadaForaPonta)
                   .HasColumnName("DemandaContradadaForaPonta")
                   .IsRequired(false);

            builder.Property(x => x.DemandaContradadaPonta)
                   .HasColumnName("DemandaContradadaPonta")
                   .IsRequired(false);

            builder.Property(x => x.ValorGestao)
                   .HasColumnName("ValorGestao")
                   .IsRequired(false);

            builder.Property(x => x.CreatorUserId)
                   .HasColumnName("CreatorUserId")
                   .IsRequired(false);

            builder.Property(x => x.DataAssinatura)
                   .HasColumnName("DataAssinatura")
                   .IsRequired(false);

            builder.Property(x => x.DeleterUserId)
                   .HasColumnName("DeleterUserId")
                   .IsRequired(false);

            builder.Property(x => x.DeletionTime)
                   .HasColumnName("DeletionTime")
                   .IsRequired(false);

            builder.Property(x => x.IsDeleted)
                   .HasColumnName("IsDeleted")
                   .IsRequired(true);

            builder.Property(x => x.LastModificationTime)
                   .HasColumnName("LastModificationTime")
                   .IsRequired(false);

            builder.Property(x => x.LastModifierUserId)
                   .HasColumnName("LastModifierUserId")
                   .IsRequired(false);

            builder.Property(x => x.SenhaDist)
                   .HasColumnName("SenhaDist")
                   .IsRequired(false);

            builder.HasOne(e => e.State)
                   .WithMany(e => e.ConsumerUnit)
                   .HasForeignKey(cu => cu.StateId)
                   .HasPrincipalKey(s => s.Id);

            builder.HasOne(e => e.Customer)
                   .WithMany(e => e.ConsumerUnit)
                   .HasForeignKey(cu => cu.CustomerId)
                   .HasPrincipalKey(s => s.Id);

            builder.HasOne(e => e.Subgroup)
                   .WithMany(e => e.ConsumerUnit)
                   .HasForeignKey(cu => cu.SubgroupId)
                   .HasPrincipalKey(s => s.Id);

            builder.HasMany(cu => cu.ConsumerUnitMeasurements)
                   .WithOne(m => m.ConsumerUnit)
                   .HasForeignKey(m => m.ConsumerUnitId)
                   .HasPrincipalKey(cu => cu.Id);



        }
    }
}
