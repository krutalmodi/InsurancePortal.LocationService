using InsurancePortal.LocationService.Application.Abstractions;
using InsurancePortal.LocationService.Application.DTOs.Responses;
using InsurancePortal.LocationService.Application.Repositories;

namespace InsurancePortal.LocationService.Application.Queries
{
    public class GetProvincesByCountryIdQueryHandler : IQueryHandler<GetProvincesByCountryIdQuery, IEnumerable<ProvinceLookupResponse>>
    {
        private readonly ILookupRepository _repository;

        public GetProvincesByCountryIdQueryHandler(ILookupRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<ProvinceLookupResponse>> HandleAsync(GetProvincesByCountryIdQuery query)
        {
            var provinces = await _repository.GetProvincesByCountryIdAsync(query.CountryId);
            return provinces.Select(province => new ProvinceLookupResponse(
                province.Id, province.Code, province.Name, province.Description, province.CountryId, province.ProvinceCode));
        }
    }
}
