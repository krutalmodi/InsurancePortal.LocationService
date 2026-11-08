using InsurancePortal.LocationService.Application.Abstractions;
using InsurancePortal.LocationService.Application.Repositories;

namespace InsurancePortal.LocationService.Application.Commands
{
    public class DeleteLocationCommandHandler : ICommandHandler<DeleteLocationCommand, bool>
    {
        private readonly ILocationRepository _repository;

        public DeleteLocationCommandHandler(ILocationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> HandleAsync(DeleteLocationCommand command)
        {
            var location = await _repository.GetByIdAsync(command.Id);
            if (location is null)
                return false;

            await _repository.DeleteAsync(command.Id);
            return true;
        }
    }
}
