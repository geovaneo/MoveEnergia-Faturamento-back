using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class SubgroupMap : IEntityTypeConfiguration<Subgroup>
    {
        public void Configure(EntityTypeBuilder<Subgroup> builder)
        {
            builder.ToTable("Subgroup", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.Description)
                   .HasColumnName("Description")
                   .IsRequired(false);

        }
    }
}
