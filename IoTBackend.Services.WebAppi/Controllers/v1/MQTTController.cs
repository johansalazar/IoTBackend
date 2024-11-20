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
    public class MQTTController : ControllerBase
    {
        private readonly MQTTUseCases _mqttUseCases;

        public MQTTController(MQTTUseCases mqttUseCases)
        {
            _mqttUseCases = mqttUseCases;
        }

        [HttpPost("AddMQTT")]
        public async Task<IActionResult> AddMQTT([FromBody] MQTTDTO mqttDto)
        {
            if (mqttDto == null) return BadRequest();

            var response = await _mqttUseCases.AddMQTTAsync(mqttDto);
            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMQTT(Guid id)
        {
            var response = await _mqttUseCases.GetMQTTByIdAsync(id);
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }

        [HttpPut("UpdateMQTT")]
        public async Task<IActionResult> UpdateMQTT([FromBody] MQTTDTO mqttDto)
        {
            if (mqttDto == null) return BadRequest();

            var response = await _mqttUseCases.UpdateMQTTAsync(mqttDto);
            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMQTT(Guid id)
        {
            var response = await _mqttUseCases.DeleteMQTTAsync(id);
            if (response.IsSuccess) return Ok(response);

            return NotFound(response.Message);
        }
    }
}
