using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class DealsMap : IEntityTypeConfiguration<Deals>
    {
        public void Configure(EntityTypeBuilder<Deals> builder)
        {
            builder.ToTable("Deals", "dbo");
            
            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                  .HasColumnName("Id")
                  .IsRequired(true);
            
            builder.Property(x => x.DealId)
                  .HasColumnName("DealId")
                  .IsRequired(false);
            
            builder.Property(x => x.Name)
                  .HasColumnName("Name")
                  .IsRequired(false);
            
            builder.Property(x => x.Status)
                  .HasColumnName("Status")
                  .IsRequired(false);

            builder.Property(x => x.Value)
                  .HasColumnName("Value")
                  .IsRequired(true);

            builder.Property(x => x.CreatedAt)
                   .HasColumnName("CreatedAt")
                  .IsRequired(true);

            builder.Property(x => x.UpdatedAt)
                   .HasColumnName("UpdatedAt")
                   .IsRequired(true);

            builder.Property(x => x.CreationTime)
                   .HasColumnName("CreationTime")
                   .IsRequired(true);

            builder.Property(x => x.CreatorUserID)
                   .HasColumnName("CreatorUserID")
                   .IsRequired(false);

            builder.Property(x => x.LastModificationTime)
                   .HasColumnName("LastModificationTime")
                   .IsRequired(false);

            builder.Property(x => x.IsDeleted)
                   .HasColumnName("IsDeleted")
                   .IsRequired(true);

            builder.Property(x => x.DeleterUserId)
                   .HasColumnName("DeleterUserId")
                   .IsRequired(false);

            builder.Property(x => x.DeletionTime)
                   .HasColumnName("DeletionTime")
                   .IsRequired(false);

            builder.Property(x => x.CNPJCPF)
                   .HasColumnName("Deal_custom_field_CNPJ_CPF")
                   .IsRequired(false);

            builder.Property(x => x.Distribuidora)
                   .HasColumnName("Deal_custom_field_Distribuidora")
                   .IsRequired(false);
           
            builder.Property(x => x.SubOrigem)
                   .HasColumnName("Deal_custom_field_Suborigem")
                   .IsRequired(false);
            
            builder.Property(x => x.UC)
                   .HasColumnName("Deal_custom_field_UC")
                   .IsRequired(false);
           
            builder.Property(x => x.Pipeline)
                   .HasColumnName("Deal_pipeline_name")
                   .IsRequired(false);

            builder.Property(x => x.Stage)
                   .HasColumnName("Deal_pipeline_name")
                   .IsRequired(false);

            builder.Property(x => x.DealsWin)
                   .HasColumnName("Deals_Win")
                   .IsRequired(false);

            builder.Property(x => x.FechadoEm)
                   .HasColumnName("FechadoEm")
                   .IsRequired(false);

            builder.Property(x => x.CsAutoConsumo)
                   .HasColumnName("Deal_custom_field_CS_Autoconsumo")
                   .IsRequired(false);

            builder.Property(x => x.CoopConsumoMedioKWh)
                   .HasColumnName("Deal_custom_field_ConsumidorCoopConsumoMedioKWh")
                   .IsRequired(false);

            builder.Property(x => x.Titularidade)
                   .HasColumnName("Deal_custom_field_Titularidade")
                   .IsRequired(false);

            builder.Property(x => x.CEP)
                   .HasColumnName("Deal_custom_field_CEP")
                   .IsRequired(false);

            builder.Property(x => x.CoopModalidadeDesconto)
                   .HasColumnName("Deal_custom_field_CoopModalidadeDesconto")
                   .IsRequired(false);

            builder.Property(x => x.DescAdicionalPercent)
                   .HasColumnName("Deal_custom_field_DescAdicionalPercent")
                   .IsRequired(false);

            builder.Property(x => x.DescAmarela)
                   .HasColumnName("Deal_custom_field_DescAmarela")
                   .IsRequired(false);

            builder.Property(x => x.DescVerde)
                   .HasColumnName("Deal_custom_field_DescVerde")
                   .IsRequired(false);

            builder.Property(x => x.DescVermelha1)
                   .HasColumnName("Deal_custom_field_DescVermelha1")
                   .IsRequired(false);

            builder.Property(x => x.DescVermelha2)
                   .HasColumnName("Deal_custom_field_DescVermelha2")
                   .IsRequired(false);
        }
    }
}
