using System.ComponentModel.DataAnnotations;
using InsurancePortal.LocationService.Application.Abstractions;
using InsurancePortal.LocationService.Application.DTOs.Responses;
using InsurancePortal.LocationService.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePortal.LocationService.API.Controllers
{
    [ApiController]
    [Route("api/lookups")]
    [AllowAnonymous]
    public class LookupsController : ControllerBase
    {
        private readonly IQueryHandler<GetCountriesQuery, IEnumerable<CountryLookupResponse>> _getCountries;
        private readonly IQueryHandler<GetProvincesByCountryIdQuery, IEnumerable<ProvinceLookupResponse>> _getProvinces;
        private readonly IQueryHandler<GetLocationTypesQuery, IEnumerable<LocationTypeLookupResponse>> _getLocationTypes;

        public LookupsController(
            IQueryHandler<GetCountriesQuery, IEnumerable<CountryLookupResponse>> getCountries,
            IQueryHandler<GetProvincesByCountryIdQuery, IEnumerable<ProvinceLookupResponse>> getProvinces,
            IQueryHandler<GetLocationTypesQuery, IEnumerable<LocationTypeLookupResponse>> getLocationTypes)
        {
            _getCountries = getCountries;
            _getProvinces = getProvinces;
            _getLocationTypes = getLocationTypes;
        }

        [HttpGet("countries")]
        public async Task<ActionResult<IEnumerable<CountryLookupResponse>>> GetCountries()
        {
            return Ok(await _getCountries.HandleAsync(new GetCountriesQuery()));
        }

        [HttpGet("countries/{countryId:int}/provinces")]
        public async Task<ActionResult<IEnumerable<ProvinceLookupResponse>>> GetProvincesByCountryId([Range(1, int.MaxValue)] int countryId)
        {
            return Ok(await _getProvinces.HandleAsync(new GetProvincesByCountryIdQuery(countryId)));
        }

        [HttpGet("location-types")]
        public async Task<ActionResult<IEnumerable<LocationTypeLookupResponse>>> GetLocationTypes()
        {
            return Ok(await _getLocationTypes.HandleAsync(new GetLocationTypesQuery()));
        }
    }
}
