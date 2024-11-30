using Asp.Versioning;
using IoTBackend.Aplication.DTOs;
using IoTBackend.Aplication.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IoTBackend.Services.WebAppi.Controllers.v1
{
    /// <summary>
    /// Servicio para MQTT
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class MQTTController : ControllerBase
    {
        private readonly MQTTUseCases _mqttUseCases;

        /// <summary>
        /// Casos de uso de MQTT
        /// </summary>
        /// <param name="mqttUseCases"></param>
        public MQTTController(MQTTUseCases mqttUseCases)
        {
            _mqttUseCases = mqttUseCases;
        }

        /// <summary>
        /// Servicio para Crear los MQTTS
        /// </summary>
        /// <param name="mqttDto"></param>
        /// <returns></returns>
        [HttpPost("AddMQTT")]
        public async Task<IActionResult> AddMQTT([FromBody] MQTTDTO mqttDto)
        {
            if (mqttDto == null) return BadRequest();

            var response = await _mqttUseCases.AddMQTTAsync(mqttDto);
            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Servicio para Elimiar los MQTT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteMQTT/{id}")]
        public async Task<IActionResult> DeleteMQTT(string id)
        {
            var response = await _mqttUseCases.DeleteMQTTAsync(id);
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }
        /// <summary>
        /// Servicio para obtener los MQTTS
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllMQTTs")]
        public async Task<IActionResult> GetAllMQTTs()
        {
            var response = await _mqttUseCases.GetAllMQTTsAsync();
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }
               
        /// <summary>
        /// Servicio para obtener el MQTT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetMQTT/{id}")]
        public async Task<IActionResult> GetMQTT(string id)
        {
            var response = await _mqttUseCases.GetMQTTByIdAsync(id);
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }

        /// <summary>
        /// Servicio para actualizar los MQTT
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mqttDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateMQTT/{id}")]
        public async Task<IActionResult> UpdateMQTT(string id, [FromBody] MQTTDTO mqttDto)
        {
            if (mqttDto == null) return BadRequest();
            mqttDto.Id = Guid.Parse(id); 
            var response = await _mqttUseCases.UpdateMQTTAsync(id,mqttDto);
            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Message);
        }
               
    }
}
