using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class AddressMap : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.CEP)
                   .HasColumnName("CEP")
                   .IsRequired(true);

            builder.Property(x => x.Logradouro)
                   .HasColumnName("Logradouro")
                   .IsRequired(true);

            builder.Property(x => x.Numero)
                   .HasColumnName("Numero")
                   .IsRequired(true);

            builder.Property(x => x.Complemento)
                   .HasColumnName("Complemento")
                   .IsRequired(false);

            builder.Property(x => x.Bairro)
                   .HasColumnName("Bairro")
                   .IsRequired(true);

            builder.Property(x => x.CityId)
                   .HasColumnName("CityId")
                   .IsRequired(true);

            builder.Property(x => x.CustomerId)
                   .HasColumnName("CustomerId")
                   .IsRequired(true);

            builder.HasOne(e => e.City)
                   .WithOne(e => e.Address)
                   .HasForeignKey<Address>(e => e.CityId);

            builder.HasOne(e => e.Customer)
                   .WithMany(e => e.Addresses)
                   .HasForeignKey(e => e.CustomerId);


        }
    }
}
