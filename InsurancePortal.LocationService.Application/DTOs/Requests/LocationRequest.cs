namespace InsurancePortal.LocationService.Application.DTOs.Requests
{
    /// <summary>
    /// Request DTO for creating or updating a location
    /// </summary>
    public class LocationRequest
    {
        /// <summary>
        /// Type of location (Residential, Commercial, etc.) - FK to LocationTypeLookup
        /// </summary>
        public Guid LocationTypeId { get; set; }

        /// <summary>
        /// Unit/Suite number (optional)
        /// </summary>
        public string? UnitNumber { get; set; }

        /// <summary>
        /// Primary street address line
        /// </summary>
        public string StreetAddress1 { get; set; } = string.Empty;

        /// <summary>
        /// Secondary street address line (apartment, building, etc.)
        /// </summary>
        public string? StreetAddress2 { get; set; }

        /// <summary>
        /// City/Municipality
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Postal/ZIP code
        /// </summary>
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// FK to ProvinceLookup
        /// </summary>
        public Guid ProvinceId { get; set; }

        /// <summary>
        /// FK to CountryLookup
        /// </summary>
        public Guid CountryId { get; set; }
    }
}
