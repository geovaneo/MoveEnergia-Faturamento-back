using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class ConsumerUnitMeasurementMap : IEntityTypeConfiguration<ConsumerUnitMeasurement>
    {
        public void Configure(EntityTypeBuilder<ConsumerUnitMeasurement> builder)
        {
            builder.ToTable("ConsumerUnitMeasurements", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.Date)
                   .HasColumnName("Date")
                   .IsRequired(true);

            builder.Property(x => x.Value)
                   .HasColumnName("Value")
                   .IsRequired(false);

            builder.Property(x => x.Ponta)
                   .HasColumnName("Ponta")
                   .IsRequired(false);

            builder.Property(x => x.ForaPonta)
                   .HasColumnName("ForaPonta")
                   .IsRequired(false);

            builder.Property(x => x.ConsumerUnitId)
                   .HasColumnName("ConsumerUnitId")
                   .IsRequired(true);

            builder.Property(x => x.CreatorUserId)
                   .HasColumnName("CreatorUserId")
                   .IsRequired(false);

            builder.Property(x => x.CreationTime)
                   .HasColumnName("CreationTime")
                   .IsRequired(true);

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

            //builder.HasOne(e => e.ConsumerUnit)
            //       .WithOne(e => e.ConsumerUnitMeasurements)
            //       .HasForeignKey<ConsumerUnit>(e => e.CustomerId);

        }
    }
}
