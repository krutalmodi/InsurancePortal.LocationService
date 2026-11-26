namespace InsurancePortal.LocationService.Application.DTOs.Responses
{
    public record ProvinceLookupResponse(int Id, string Code, string Name, string? Description, int CountryId, string ProvinceCode);
}
