using InsurancePortal.LocationService.Application.Repositories;
using InsurancePortal.LocationService.Domain.Entities.Lookups;
using InsurancePortal.LocationService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InsurancePortal.LocationService.Infrastructure.Repositories
{
    public class LookupRepository : ILookupRepository
    {
        private readonly LocationDbContext _context;

        public LookupRepository(LocationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<CountryLookup>> GetCountriesAsync()
        {
            return await _context.CountryLookups.AsNoTracking()
                .Where(country => country.IsActive)
                .OrderBy(country => country.SortOrder).ThenBy(country => country.Name).ThenBy(country => country.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProvinceLookup>> GetProvincesByCountryIdAsync(int countryId)
        {
            return await _context.ProvinceLookups.AsNoTracking()
                .Where(province => province.CountryId == countryId && province.IsActive && province.Country != null && province.Country.IsActive)
                .OrderBy(province => province.SortOrder).ThenBy(province => province.Name).ThenBy(province => province.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<LocationTypeLookup>> GetLocationTypesAsync()
        {
            return await _context.LocationTypeLookups.AsNoTracking()
                .Where(locationType => locationType.IsActive)
                .OrderBy(locationType => locationType.SortOrder).ThenBy(locationType => locationType.Name).ThenBy(locationType => locationType.Id)
                .ToListAsync();
        }
    }
}
