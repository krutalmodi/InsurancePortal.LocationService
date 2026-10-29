namespace InsurancePortal.LocationService.Domain.Entities
{
    /// <summary>
    /// Represents a physical address location
    /// Owned by LocationService; QuoteService stores a LocationId plus an address snapshot
    /// </summary>
    public class Location
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Foreign key to LocationTypeLookup (Residential, Commercial, etc.)
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
        /// Foreign key to ProvinceLookup
        /// </summary>
        public Guid ProvinceId { get; set; }

        /// <summary>
        /// Foreign key to CountryLookup
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Audit: When created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Audit: Who/what created this record
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Audit: When last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Audit: Who/what last updated this record
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// Get full address formatted string
        /// </summary>
        public string GetFullAddress()
        {
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(UnitNumber))
                parts.Add(UnitNumber);
            parts.Add(StreetAddress1);
            if (!string.IsNullOrEmpty(StreetAddress2))
                parts.Add(StreetAddress2);
            return string.Join(", ", parts);
        }
    }
}
