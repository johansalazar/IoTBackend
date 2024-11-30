using Asp.Versioning;
using IoTBackend.Aplication.DTOs;
using IoTBackend.Aplication.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IoTBackend.Services.WebAppi.Controllers.v1
{
    /// <summary>
    /// Api de Ubicaciones
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private readonly LocationUseCases _locationUseCases;
        /// <summary>
        /// Casos de uso de ubicaciones
        /// </summary>
        /// <param name="locationUseCases"></param>
        public LocationsController(LocationUseCases locationUseCases)
        {
            _locationUseCases = locationUseCases;
        }

        /// <summary>
        /// Servicio para la creacion de ubicaciones
        /// </summary>
        /// <param name="locationDto"></param>
        /// <returns></returns>
        [HttpPost("AddLocation")]
        public async Task<IActionResult> AddLocation([FromBody] LocationDTO locationDto)
        {
            if (locationDto == null) return BadRequest();

            var response = await _locationUseCases.AddLocationAsync(locationDto);
            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Servicio para eliminar las ubicaciones
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteLocation/{id}")]
        public async Task<IActionResult> DeleteLocation(string id)
        {
            var response = await _locationUseCases.DeleteLocationAsync(id);
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }

        /// <summary>
        /// Servicio para obtener todas las ubicaciones
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllLocations")]
        public async Task<IActionResult> GetAllLocations()
        {
            var response = await _locationUseCases.GetAllLocationsAsync();
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }

        /// <summary>
        /// Servicio para obtener la ubicacion por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetLocation/{id}")]        
        public async Task<IActionResult> GetLocation(string id)
        {
            var response = await _locationUseCases.GetLocationByIdAsync(id);
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }

        /// <summary>
        /// Servicio para actualizar las ubicaciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="locationDTO"></param>
        /// <returns></returns>
        [HttpPut("UpdateLocation/{id}")]
        public async Task<IActionResult> UpdateLocation(string id, LocationDTO locationDTO)
        {
            if (locationDTO == null) return BadRequest();
            locationDTO.Id = Guid.Parse(id);
            var response = await _locationUseCases.UpdateLocationAsync(id, locationDTO);
            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Message);
        }
    }
}
