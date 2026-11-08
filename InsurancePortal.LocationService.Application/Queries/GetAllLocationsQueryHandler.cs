using InsurancePortal.LocationService.Application.Abstractions;
using InsurancePortal.LocationService.Application.DTOs.Responses;
using InsurancePortal.LocationService.Application.Mappings;
using InsurancePortal.LocationService.Application.Repositories;

namespace InsurancePortal.LocationService.Application.Queries
{
    public class GetAllLocationsQueryHandler : IQueryHandler<GetAllLocationsQuery, IEnumerable<LocationResponse>>
    {
        private readonly ILocationRepository _repository;

        public GetAllLocationsQueryHandler(ILocationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<LocationResponse>> HandleAsync(GetAllLocationsQuery query)
        {
            var locations = await _repository.GetAllAsync();
            return locations.Select(LocationMapper.MapToResponse);
        }
    }
}
