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
    public class ServidoresController : ControllerBase
    {
        private readonly ServidorUseCases _servidorUseCases;

        public ServidoresController(ServidorUseCases servidorUseCases)
        {
            _servidorUseCases = servidorUseCases;
        }

        [HttpPost("AddServidor")]
        public async Task<IActionResult> AddServidor([FromBody] ServidorDTO servidorDto)
        {
            if (servidorDto == null) return BadRequest();

            var response = await _servidorUseCases.AddServidorAsync(servidorDto);
            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServidor(Guid id)
        {
            var response = await _servidorUseCases.GetServidorByIdAsync(id);
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServidores()
        {
            var response = await _servidorUseCases.GetAllServidoresAsync();
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServidor(Guid id)
        {
            var response = await _servidorUseCases.DeleteServidorAsync(id);
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateServidor([FromBody] ServidorDTO servidorDto)
        {
            if (servidorDto == null) return BadRequest();

            var response = await _servidorUseCases.UpdateServidorAsync(servidorDto);
            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Message);
        }
    }
}
