namespace InsurancePortal.LocationService.Domain.Entities
{
    /// <summary>
    /// Base class for all lookup/reference tables
    /// Provides common structure: Id, Code, Name, Description, IsActive, SortOrder, Audit fields
    /// </summary>
    public abstract class BaseLookup
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Stable, unique code identifier
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Display name for UI/reports
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional detailed description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Track active/inactive status; inactive values are retained for historical data
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Sort order for UI presentation
        /// </summary>
        public int SortOrder { get; set; }

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
    }
}
