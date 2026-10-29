namespace InsurancePortal.LocationService.Domain.Entities.Lookups
{
    /// <summary>
    /// Lookup for provinces/states
    /// Related to CountryLookup via CountryId foreign key
    /// </summary>
    public class ProvinceLookup : BaseLookup
    {
        /// <summary>
        /// Foreign key to CountryLookup
        /// Ensures province belongs to correct country
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Standardized province/state code (e.g., "CA", "ON", "BC")
        /// </summary>
        public string ProvinceCode { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property
        /// </summary>
        public virtual CountryLookup? Country { get; set; }
    }
}
