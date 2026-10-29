using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InsurancePortal.LocationService.Application.DTOs.Requests;
using InsurancePortal.LocationService.Application.DTOs.Responses;
using InsurancePortal.LocationService.Application.Services;

namespace InsurancePortal.LocationService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpPost]
        public async Task<ActionResult<LocationResponse>> Create([FromBody] LocationRequest request)
        {
            var created = await _locationService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<LocationResponse>> GetById(Guid id)
        {
            var location = await _locationService.GetByIdAsync(id);
            return location is null ? NotFound() : Ok(location);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationResponse>>> GetAll()
        {
            return Ok(await _locationService.GetAllAsync());
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<LocationResponse>> Update(Guid id, [FromBody] LocationRequest request)
        {
            var updated = await _locationService.UpdateAsync(id, request);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _locationService.DeleteAsync(id) ? NoContent() : NotFound();
        }
    }
}
