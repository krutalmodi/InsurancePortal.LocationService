using InsurancePortal.LocationService.Application.DTOs.Requests;
using InsurancePortal.LocationService.Application.DTOs.Responses;
using InsurancePortal.LocationService.Domain.Entities;

namespace InsurancePortal.LocationService.Application.Mappings
{
    public static class LocationMapper
    {
        public static void MapRequestToEntity(LocationRequest request, Location location)
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

        public static LocationResponse MapToResponse(Location location)
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
