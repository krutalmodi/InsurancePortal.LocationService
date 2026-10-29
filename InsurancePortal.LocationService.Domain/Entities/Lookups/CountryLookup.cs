namespace InsurancePortal.LocationService.Domain.Entities.Lookups
{
    /// <summary>
    /// Lookup for countries
    /// Seeded with USA and Canada
    /// </summary>
    public class CountryLookup : BaseLookup
    {
        public const string USA = "USA";
        public const string Canada = "Canada";

        /// <summary>
        /// ISO 3166-1 alpha-2 country code
        /// </summary>
        public string IsoCode { get; set; } = string.Empty;

        /// <summary>
        /// Navigation to provinces/states
        /// </summary>
        public virtual ICollection<ProvinceLookup> Provinces { get; set; } = new List<ProvinceLookup>();
    }
}
