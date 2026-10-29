namespace InsurancePortal.LocationService.Application.DTOs.Responses
{
    /// <summary>
    /// Response DTO for location details
    /// </summary>
    public class LocationResponse
    {
        public Guid Id { get; set; }

        public Guid LocationTypeId { get; set; }

        public string? UnitNumber { get; set; }

        public string StreetAddress1 { get; set; } = string.Empty;

        public string? StreetAddress2 { get; set; }

        public string City { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public Guid ProvinceId { get; set; }

        public Guid CountryId { get; set; }
    }
}
