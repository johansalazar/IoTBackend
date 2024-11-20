using Asp.Versioning;
using IoTBackend.Aplication.DTOs;
using IoTBackend.Aplication.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IoTBackend.Services.WebAppi.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private readonly LocationUseCases _locationUseCases;

        public LocationsController(LocationUseCases locationUseCases)
        {
            _locationUseCases = locationUseCases;
        }

        [HttpPost("AddLocation")]
        public async Task<IActionResult> AddLocation([FromBody] LocationDTO locationDto)
        {
            if (locationDto == null) return BadRequest();

            var response = await _locationUseCases.AddLocationAsync(locationDto);
            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocation(Guid id)
        {
            var response = await _locationUseCases.GetLocationByIdAsync(id);
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLocations()
        {
            var response = await _locationUseCases.GetAllLocationsAsync();
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            var response = await _locationUseCases.DeleteLocationAsync(id);
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLocation([FromBody] LocationDTO locationDTO)
        {
            if (locationDTO == null) return BadRequest();

            var response = await _locationUseCases.UpdateLocationAsync(locationDTO);
            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Message);
        }
    }
}
