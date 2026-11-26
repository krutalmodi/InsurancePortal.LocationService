using InsurancePortal.LocationService.Application.Abstractions;
using InsurancePortal.LocationService.Application.DTOs.Responses;
using InsurancePortal.LocationService.Application.Repositories;

namespace InsurancePortal.LocationService.Application.Queries
{
    public class GetCountriesQueryHandler : IQueryHandler<GetCountriesQuery, IEnumerable<CountryLookupResponse>>
    {
        private readonly ILookupRepository _repository;

        public GetCountriesQueryHandler(ILookupRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<CountryLookupResponse>> HandleAsync(GetCountriesQuery query)
        {
            var countries = await _repository.GetCountriesAsync();
            return countries.Select(country => new CountryLookupResponse(
                country.Id, country.Code, country.Name, country.Description, country.IsoCode));
        }
    }
}
