using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class FederativeUnitMap : IEntityTypeConfiguration<FederativeUnit>
    {
        public void Configure(EntityTypeBuilder<FederativeUnit> builder)
        {
            builder.ToTable("FederativeUnits", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.Nome)
                   .HasColumnName("Nome")
                   .IsRequired(true);

            builder.Property(x => x.Sigla)
                   .HasColumnName("Sigla")
                   .IsRequired(false);

            builder.Property(x => x.CountryId)
                   .HasColumnName("CountryId")
                   .IsRequired(true);

            builder.HasOne(c => c.City)
                   .WithOne(a => a.UF)
                   .HasForeignKey<City>(a => a.Id);
        }
    }
}
