using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Data.Mappings
{
    public class TelephoneMap : IEntityTypeConfiguration<Telephone>
    {
        public void Configure(EntityTypeBuilder<Telephone> builder)
        {
            builder.ToTable("Telephones", "dbo");

            builder.HasKey(e => e.Id)
                   .HasName("Id");

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired(true);

            builder.Property(x => x.ContactId)
                   .HasColumnName("ContactId")
                   .IsRequired(true);
        }
    }
}
