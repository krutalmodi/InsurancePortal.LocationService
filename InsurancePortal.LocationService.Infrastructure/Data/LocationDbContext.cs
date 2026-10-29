using Microsoft.EntityFrameworkCore;
using InsurancePortal.LocationService.Domain.Entities;
using InsurancePortal.LocationService.Domain.Entities.Lookups;
using InsurancePortal.LocationService.Infrastructure.Data.Configurations;
using InsurancePortal.LocationService.Infrastructure.Data.Configurations.Lookups;

namespace InsurancePortal.LocationService.Infrastructure.Data
{
    /// <summary>
    /// Entity Framework Core DbContext for Location Service
    /// Handles Location records and the Location-owned lookup tables
    /// </summary>
    public class LocationDbContext : DbContext
    {
        public LocationDbContext(DbContextOptions<LocationDbContext> options) : base(options)
        {
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationTypeLookup> LocationTypeLookups { get; set; }
        public DbSet<CountryLookup> CountryLookups { get; set; }
        public DbSet<ProvinceLookup> ProvinceLookups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new LocationTypeLookupConfiguration());
            modelBuilder.ApplyConfiguration(new CountryLookupConfiguration());
            modelBuilder.ApplyConfiguration(new ProvinceLookupConfiguration());
        }
    }
}
