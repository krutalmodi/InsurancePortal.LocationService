using InsurancePortal.LocationService.Application.DTOs.Requests;
using InsurancePortal.LocationService.Application.DTOs.Responses;
using InsurancePortal.LocationService.Application.Repositories;
using InsurancePortal.LocationService.Domain.Entities;

namespace InsurancePortal.LocationService.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repository;

        public LocationService(ILocationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<LocationResponse> CreateAsync(LocationRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var location = new Location();
            MapRequestToEntity(request, location);
            location.CreatedAt = DateTime.UtcNow;
            location.UpdatedAt = location.CreatedAt;

            await _repository.AddAsync(location);
            return MapToResponse(location);
        }

        public async Task<LocationResponse?> GetByIdAsync(Guid id)
        {
            var location = await _repository.GetByIdAsync(id);
            return location is null ? null : MapToResponse(location);
        }

        public async Task<IEnumerable<LocationResponse>> GetAllAsync()
        {
            var locations = await _repository.GetAllAsync();
            return locations.Select(MapToResponse);
        }

        public async Task<LocationResponse?> UpdateAsync(Guid id, LocationRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var location = await _repository.GetByIdAsync(id);
            if (location is null)
                return null;

            MapRequestToEntity(request, location);
            location.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(location);
            return MapToResponse(location);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var location = await _repository.GetByIdAsync(id);
            if (location is null)
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        private static void MapRequestToEntity(LocationRequest request, Location location)
        {
            location.LocationTypeId = request.LocationTypeId;
            location.UnitNumber = request.UnitNumber;
            location.StreetAddress1 = request.StreetAddress1;
            location.StreetAddress2 = request.StreetAddress2;
            location.City = request.City;
            location.PostalCode = request.PostalCode;
            location.ProvinceId = request.ProvinceId;
            location.CountryId = request.CountryId;
        }

        private static LocationResponse MapToResponse(Location location)
        {
            return new LocationResponse
            {
                Id = location.Id,
                LocationTypeId = location.LocationTypeId,
                UnitNumber = location.UnitNumber,
                StreetAddress1 = location.StreetAddress1,
                StreetAddress2 = location.StreetAddress2,
                City = location.City,
                PostalCode = location.PostalCode,
                ProvinceId = location.ProvinceId,
                CountryId = location.CountryId
            };
        }
    }
}
