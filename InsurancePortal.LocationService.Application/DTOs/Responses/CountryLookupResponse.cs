namespace InsurancePortal.LocationService.Application.DTOs.Responses
{
    public record CountryLookupResponse(int Id, string Code, string Name, string? Description, string IsoCode);
}
