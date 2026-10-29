using Microsoft.EntityFrameworkCore;
using InsurancePortal.LocationService.Application.Repositories;
using InsurancePortal.LocationService.Domain.Entities;
using InsurancePortal.LocationService.Infrastructure.Data;

namespace InsurancePortal.LocationService.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for Location persistence
    /// Handles all CRUD operations for Location entities
    /// </summary>
    public class LocationRepository : ILocationRepository
    {
        private readonly LocationDbContext _context;

        public LocationRepository(LocationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Add a new location to the database
        /// </summary>
        public async Task AddAsync(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            if (location.Id == Guid.Empty)
                location.Id = Guid.NewGuid();

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update an existing location
        /// </summary>
        public async Task UpdateAsync(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            _context.Locations.Update(location);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get location by ID
        /// </summary>
        public async Task<Location?> GetByIdAsync(Guid id)
        {
            return await _context.Locations.FindAsync(id);
        }

        /// <summary>
        /// Get all locations
        /// </summary>
        public async Task<IEnumerable<Location>> GetAllAsync()
        {
            return await _context.Locations.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Delete a location by ID
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location is not null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
            }
        }
    }
}
