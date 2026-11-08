using InsurancePortal.LocationService.Application.DTOs.Requests;

namespace InsurancePortal.LocationService.Application.Commands
{
    public record CreateLocationCommand(LocationRequest Request);
}
