using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InsurancePortal.LocationService.Application.Abstractions;
using InsurancePortal.LocationService.Application.Commands;
using InsurancePortal.LocationService.Application.DTOs.Requests;
using InsurancePortal.LocationService.Application.DTOs.Responses;
using InsurancePortal.LocationService.Application.Queries;

namespace InsurancePortal.LocationService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private readonly ICommandHandler<CreateLocationCommand, LocationResponse> _createLocation;
        private readonly ICommandHandler<UpdateLocationCommand, LocationResponse?> _updateLocation;
        private readonly ICommandHandler<DeleteLocationCommand, bool> _deleteLocation;
        private readonly IQueryHandler<GetLocationByIdQuery, LocationResponse?> _getLocationById;
        private readonly IQueryHandler<GetAllLocationsQuery, IEnumerable<LocationResponse>> _getAllLocations;

        public LocationsController(
            ICommandHandler<CreateLocationCommand, LocationResponse> createLocation,
            ICommandHandler<UpdateLocationCommand, LocationResponse?> updateLocation,
            ICommandHandler<DeleteLocationCommand, bool> deleteLocation,
            IQueryHandler<GetLocationByIdQuery, LocationResponse?> getLocationById,
            IQueryHandler<GetAllLocationsQuery, IEnumerable<LocationResponse>> getAllLocations)
        {
            _createLocation = createLocation;
            _updateLocation = updateLocation;
            _deleteLocation = deleteLocation;
            _getLocationById = getLocationById;
            _getAllLocations = getAllLocations;
        }

        [HttpPost]
        public async Task<ActionResult<LocationResponse>> Create([FromBody] LocationRequest request)
        {
            var created = await _createLocation.HandleAsync(new CreateLocationCommand(request));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<LocationResponse>> GetById(Guid id)
        {
            var location = await _getLocationById.HandleAsync(new GetLocationByIdQuery(id));
            return location is null ? NotFound() : Ok(location);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationResponse>>> GetAll()
        {
            return Ok(await _getAllLocations.HandleAsync(new GetAllLocationsQuery()));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<LocationResponse>> Update(Guid id, [FromBody] LocationRequest request)
        {
            var updated = await _updateLocation.HandleAsync(new UpdateLocationCommand(id, request));
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _deleteLocation.HandleAsync(new DeleteLocationCommand(id)) ? NoContent() : NotFound();
        }
    }
}
