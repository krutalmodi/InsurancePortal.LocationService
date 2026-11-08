using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InsurancePortal.LocationService.Domain.Entities.Lookups;

namespace InsurancePortal.LocationService.Infrastructure.Data.Configurations.Lookups
{
    public class ProvinceLookupConfiguration : IEntityTypeConfiguration<ProvinceLookup>
    {
        public void Configure(EntityTypeBuilder<ProvinceLookup> builder)
        {
            builder.ToTable("ProvinceLookups");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(x => x.Code).IsUnique();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.SortOrder)
                .HasDefaultValue(0);

            builder.Property(x => x.CreatedAt)
                .HasColumnType("datetime2");

            builder.Property(x => x.UpdatedAt)
                .HasColumnType("datetime2");

            builder.Property(x => x.CountryId)
                .IsRequired();

            builder.Property(x => x.ProvinceCode)
                .IsRequired()
                .HasMaxLength(5);

            builder.HasIndex(x => new { x.CountryId, x.ProvinceCode })
                .IsUnique();

            builder.HasOne(x => x.Country)
                .WithMany(c => c.Provinces)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            const int usa = 1;
            const int canada = 2;
            builder.HasData(
                Province(1, usa, "CA", "California", 1, seedDate),
                Province(2, usa, "TX", "Texas", 2, seedDate),
                Province(3, usa, "NY", "New York", 3, seedDate),
                Province(4, usa, "FL", "Florida", 4, seedDate),
                Province(5, canada, "ON", "Ontario", 5, seedDate),
                Province(6, canada, "QC", "Quebec", 6, seedDate),
                Province(7, canada, "BC", "British Columbia", 7, seedDate),
                Province(8, canada, "AB", "Alberta", 8, seedDate),
                Province(9, canada, "MB", "Manitoba", 9, seedDate));
        }

        private static object Province(int id, int countryId, string code, string name, int sortOrder, DateTime seedDate) => new
        {
            Id = id,
            CountryId = countryId,
            Code = code,
            ProvinceCode = code,
            Name = name,
            Description = (string?)null,
            IsActive = true,
            SortOrder = sortOrder,
            CreatedAt = seedDate,
            UpdatedAt = seedDate
        };
    }
}
