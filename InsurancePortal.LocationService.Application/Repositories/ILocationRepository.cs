using InsurancePortal.LocationService.Domain.Entities;

namespace InsurancePortal.LocationService.Application.Repositories
{
    /// <summary>
    /// Repository interface for Location persistence
    /// </summary>
    public interface ILocationRepository
    {
        Task AddAsync(Location location);

        Task UpdateAsync(Location location);

        Task<Location?> GetByIdAsync(Guid id);

        Task<Location?> GetByLocationAsync(Location location);

        Task<IEnumerable<Location>> GetAllAsync();

        Task DeleteAsync(Guid id);
    }
}
