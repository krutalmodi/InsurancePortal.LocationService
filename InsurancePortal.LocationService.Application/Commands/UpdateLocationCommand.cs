using InsurancePortal.LocationService.Application.DTOs.Requests;

namespace InsurancePortal.LocationService.Application.Commands
{
    public record UpdateLocationCommand(Guid Id, LocationRequest Request);
}
