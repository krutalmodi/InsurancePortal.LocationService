using InsurancePortal.LocationService.Domain.Entities.Lookups;

namespace InsurancePortal.LocationService.Application.Repositories
{
    public interface ILookupRepository
    {
        Task<IEnumerable<CountryLookup>> GetCountriesAsync();

        Task<IEnumerable<ProvinceLookup>> GetProvincesByCountryIdAsync(int countryId);

        Task<IEnumerable<LocationTypeLookup>> GetLocationTypesAsync();
    }
}
