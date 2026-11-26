using InsurancePortal.LocationService.Application.Abstractions;
using InsurancePortal.LocationService.Application.DTOs.Responses;
using InsurancePortal.LocationService.Application.Repositories;

namespace InsurancePortal.LocationService.Application.Queries
{
    public class GetLocationTypesQueryHandler : IQueryHandler<GetLocationTypesQuery, IEnumerable<LocationTypeLookupResponse>>
    {
        private readonly ILookupRepository _repository;

        public GetLocationTypesQueryHandler(ILookupRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<LocationTypeLookupResponse>> HandleAsync(GetLocationTypesQuery query)
        {
            var locationTypes = await _repository.GetLocationTypesAsync();
            return locationTypes.Select(locationType => new LocationTypeLookupResponse(
                locationType.Id, locationType.Code, locationType.Name, locationType.Description));
        }
    }
}
