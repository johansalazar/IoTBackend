﻿
using Asp.Versioning;
using IoTBackend.Aplication.DTOs;
using IoTBackend.Aplication.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoTBackend.Services.WebAppi.Controllers.v1
{
    /// <summary>
    /// Controlador para la gestión de dispositivos IoT.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class DevicesController(DeviceUseCases deviceUseCases) : ControllerBase
    {
        /// <summary>
        /// Método para crear un dispositivo nuevo.
        /// </summary>
        /// <param name="devicedto">Información del dispositivo.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost("AddDeviceAsync")]
        public async Task<IActionResult> AddDevice(IoTDeviceDTO devicedto)
        {
            if (devicedto == null)
                return BadRequest();

            var response = await deviceUseCases.AddDeviceAsync(devicedto);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Método para obtener un dispositivo por su ID.
        /// </summary>
        /// <param name="id">Identificador del dispositivo.</param>
        /// <returns>El dispositivo solicitado.</returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDeviceById(Guid id)
        {
            var response = await deviceUseCases.GetDeviceByIdAsync(id);
            if (response == null)
                return NotFound("Device not found");

            return Ok(response);
        }

        /// <summary>
        /// Método para obtener todos los dispositivos.
        /// </summary>
        /// <returns>Lista de dispositivos.</returns>
        [HttpGet("GetAllDevicesAsync")]
        public async Task<IActionResult> GetAllDevices()
        {
            var response = await deviceUseCases.GetAllDevicesAsync();
            return Ok(response);
        }

        /// <summary>
        /// Método para actualizar un dispositivo existente.
        /// </summary>
        /// <param name="id">Identificador del dispositivo.</param>
        /// <param name="devicedto">Información actualizada del dispositivo.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDevice(Guid id, IoTDeviceDTO devicedto)
        {
            if (devicedto == null || id == Guid.Empty)
                return BadRequest();

            var response = await deviceUseCases.UpdateDeviceAsync(id, devicedto);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Método para eliminar un dispositivo.
        /// </summary>
        /// <param name="id">Identificador del dispositivo.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDevice(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var response = await deviceUseCases.DeleteDeviceAsync(id);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }
    }
}