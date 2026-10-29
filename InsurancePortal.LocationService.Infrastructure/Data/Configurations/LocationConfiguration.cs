using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InsurancePortal.LocationService.Domain.Entities;
using InsurancePortal.LocationService.Domain.Entities.Lookups;

namespace InsurancePortal.LocationService.Infrastructure.Data.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Locations");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.LocationTypeId)
                .IsRequired();

            builder.Property(x => x.UnitNumber)
                .HasMaxLength(50);

            builder.Property(x => x.StreetAddress1)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.StreetAddress2)
                .HasMaxLength(256);

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.PostalCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.ProvinceId)
                .IsRequired();

            builder.Property(x => x.CountryId)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(256);

            builder.Property(x => x.UpdatedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.UpdatedBy)
                .HasMaxLength(256);

            builder.HasOne<LocationTypeLookup>()
                .WithMany()
                .HasForeignKey(x => x.LocationTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ProvinceLookup>()
                .WithMany()
                .HasForeignKey(x => x.ProvinceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<CountryLookup>()
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.ProvinceId, x.City, x.PostalCode })
                .HasDatabaseName("IX_Locations_ProvinceCity_PostalCode");

            builder.HasIndex(x => x.CountryId)
                .HasDatabaseName("IX_Locations_CountryId");
        }
    }
}
