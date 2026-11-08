using InsurancePortal.LocationService.Application.Abstractions;
using InsurancePortal.LocationService.Application.DTOs.Responses;
using InsurancePortal.LocationService.Application.Mappings;
using InsurancePortal.LocationService.Application.Repositories;

namespace InsurancePortal.LocationService.Application.Commands
{
    public class UpdateLocationCommandHandler : ICommandHandler<UpdateLocationCommand, LocationResponse?>
    {
        private readonly ILocationRepository _repository;

        public UpdateLocationCommandHandler(ILocationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<LocationResponse?> HandleAsync(UpdateLocationCommand command)
        {
            ArgumentNullException.ThrowIfNull(command.Request);

            var location = await _repository.GetByIdAsync(command.Id);
            if (location is null)
                return null;

            LocationMapper.MapRequestToEntity(command.Request, location);
            location.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(location);
            return LocationMapper.MapToResponse(location);
        }
    }
}
