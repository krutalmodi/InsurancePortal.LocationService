using InsurancePortal.LocationService.Application.Abstractions;
using InsurancePortal.LocationService.Application.DTOs.Responses;
using InsurancePortal.LocationService.Application.Mappings;
using InsurancePortal.LocationService.Application.Repositories;
using InsurancePortal.LocationService.Domain.Entities;

namespace InsurancePortal.LocationService.Application.Commands
{
    public class CreateLocationCommandHandler : ICommandHandler<CreateLocationCommand, LocationResponse>
    {
        private readonly ILocationRepository _repository;

        public CreateLocationCommandHandler(ILocationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<LocationResponse> HandleAsync(CreateLocationCommand command)
        {
            ArgumentNullException.ThrowIfNull(command.Request);

            var location = new Location();
            LocationMapper.MapRequestToEntity(command.Request, location);

            var existing = await _repository.GetByLocationAsync(location);
            if (existing is not null)
                return LocationMapper.MapToResponse(existing);

            location.CreatedAt = DateTime.UtcNow;
            location.UpdatedAt = location.CreatedAt;

            await _repository.AddAsync(location);
            return LocationMapper.MapToResponse(location);
        }
    }
}
