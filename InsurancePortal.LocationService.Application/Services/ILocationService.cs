using InsurancePortal.LocationService.Application.DTOs.Requests;
using InsurancePortal.LocationService.Application.DTOs.Responses;

namespace InsurancePortal.LocationService.Application.Services
{
    public interface ILocationService
    {
        Task<LocationResponse> CreateAsync(LocationRequest request);

        Task<LocationResponse?> GetByIdAsync(Guid id);

        Task<IEnumerable<LocationResponse>> GetAllAsync();

        Task<LocationResponse?> UpdateAsync(Guid id, LocationRequest request);

        Task<bool> DeleteAsync(Guid id);
    }
}
