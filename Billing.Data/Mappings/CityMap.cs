using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class CityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.Nome)
                   .HasColumnName("Nome")
                   .IsRequired(true);

            builder.Property(x => x.UFId)
                   .HasColumnName("UFId")
                   .IsRequired(true);

            builder.HasOne(c => c.Address)
                   .WithOne(a => a.City)
                   .HasForeignKey<Address>(a => a.Id);
        }
    }
}
