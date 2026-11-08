using InsurancePortal.LocationService.Application.Abstractions;
using InsurancePortal.LocationService.Application.DTOs.Responses;
using InsurancePortal.LocationService.Application.Mappings;
using InsurancePortal.LocationService.Application.Repositories;

namespace InsurancePortal.LocationService.Application.Queries
{
    public class GetLocationByIdQueryHandler : IQueryHandler<GetLocationByIdQuery, LocationResponse?>
    {
        private readonly ILocationRepository _repository;

        public GetLocationByIdQueryHandler(ILocationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<LocationResponse?> HandleAsync(GetLocationByIdQuery query)
        {
            var location = await _repository.GetByIdAsync(query.Id);
            return location is null ? null : LocationMapper.MapToResponse(location);
        }
    }
}
