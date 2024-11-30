using Asp.Versioning;
using IoTBackend.Aplication.DTOs;
using IoTBackend.Aplication.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoTBackend.Services.WebAppi.Controllers.v1
{
    /// <summary>
    /// Servicio de Servidores
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ServidoresController : ControllerBase
    {
        private readonly ServidorUseCases _servidorUseCases;

        /// <summary>
        /// Casos de uso de servidores
        /// </summary>
        /// <param name="servidorUseCases"></param>
        public ServidoresController(ServidorUseCases servidorUseCases)
        {
            _servidorUseCases = servidorUseCases;
        }

        /// <summary>
        /// Servicio para Crear el Servidor
        /// </summary>
        /// <param name="servidorDto"></param>
        /// <returns></returns>
        [HttpPost("AddServidor")]
        public async Task<IActionResult> AddServidor([FromBody] ServidorDTO servidorDto)
        {
            if (servidorDto == null) return BadRequest();

            var response = await _servidorUseCases.AddServidorAsync(servidorDto);
            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Servicio para eliminar el servidor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteServidor/{id}")]
        public async Task<IActionResult> DeleteServidor(string id)
        {
            var response = await _servidorUseCases.DeleteServidorAsync(id);
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }             

        /// <summary>
        /// Servicio para obtener los servidores
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllServidores")]
        public async Task<IActionResult> GetAllServidores()
        {
            var response = await _servidorUseCases.GetAllServidoresAsync();
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }

        /// <summary>
        /// Servicio para obtener el servidor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetServidor/{id}")]
        public async Task<IActionResult> GetServidor(string id)
        {
            var response = await _servidorUseCases.GetServidorByIdAsync(id);
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }

        /// <summary>
        /// Servicio para Actualizar el servidor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="servidorDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateServidor/{id}")]
        public async Task<IActionResult> UpdateServidor(string id, [FromBody] ServidorDTO servidorDto)
        {
            if (servidorDto == null) return BadRequest();

            servidorDto.Id = Guid.Parse(id);
            var response = await _servidorUseCases.UpdateServidorAsync(id, servidorDto);
            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Message);
        }

    }
}
