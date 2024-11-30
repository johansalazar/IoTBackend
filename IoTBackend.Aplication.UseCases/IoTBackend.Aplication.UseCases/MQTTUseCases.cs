using AutoMapper;
using IoTBackend.Aplication.DTOs;
using IoTBackend.Aplication.Interfaces;
using IoTBackend.Domain.Dominio.Entities;
using IoTBackend.Transversal.Common;

namespace IoTBackend.Aplication.UseCases
{
    public class MQTTUseCases
    {
        private readonly IMQTTRepository _mqttRepository;
        private readonly IMapper _mapper;

        public MQTTUseCases(IMQTTRepository mqttRepository, IMapper mapper)
        {
            _mqttRepository = mqttRepository;
            _mapper = mapper;
        }

        public async Task<Response<MQTTDTO>> AddMQTTAsync(MQTTDTO mqttDto)
        {
            var response = new Response<MQTTDTO>();
            try
            {
                var mqtt = _mapper.Map<MQTT>(mqttDto);
                mqtt.Id = Guid.NewGuid(); // Asignar un nuevo GUID
                mqtt.FechaCreacion = DateTime.UtcNow;

                // Asegurarse de que el campo de auditoría esté presente
                mqtt.Auditoria = $"Creado por: Admin, Fecha: {DateTime.UtcNow}";

                var result = await _mqttRepository.AddMQTTAsync(mqtt);
                if (result)
                {
                    response.Data = _mapper.Map<MQTTDTO>(mqtt);
                    response.IsSuccess = true;
                    response.Message = "MQTT registrado con éxito.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se pudo registrar el MQTT.";
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<MQTTDTO>> GetMQTTByIdAsync(string id)
        {
            var response = new Response<MQTTDTO>();
            try
            {
                var mqtt = await _mqttRepository.GetMQTTByIdAsync(id);
                if (mqtt != null)
                {
                    response.Data = _mapper.Map<MQTTDTO>(mqtt);
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "MQTT no encontrado.";
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<IEnumerable<MQTTDTO>>> GetAllMQTTsAsync()
        {
            var response = new Response<IEnumerable<MQTTDTO>>();
            try
            {
                var mqtts = await _mqttRepository.GetAllMQTTsAsync();
                response.Data = _mapper.Map<IEnumerable<MQTTDTO>>(mqtts);
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<bool>> UpdateMQTTAsync(string id, MQTTDTO mqttDto)
        {
            var response = new Response<bool>();
            try
            {
                var mqtt = await _mqttRepository.GetMQTTByIdAsync(id);
                if (mqtt == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Usuario no encontrado.";
                    return response;
                }
                
                // Mapeo del DTO al modelo de dominio
                _mapper.Map(mqttDto, mqtt);

                var result = await
               _mqttRepository.UpdateMQTTAsync(mqtt); 
                if (result)
                {
                    response.Data = result;
                    response.IsSuccess = result;
                    response.Message = result ? "MQTT actualizado con éxito." : "No se pudo actualizar el MQTT.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se pudo actualizar el MQTT.";
                }                
                
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<bool>> DeleteMQTTAsync(string id)
        {
            var response = new Response<bool>();
            try
            {
                var result = await _mqttRepository.DeleteMQTTAsync(id);
                response.Data = result;
                response.IsSuccess = result;
                response.Message = result ? "MQTT eliminado con éxito." : "No se pudo eliminar el MQTT.";
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }
    }
}
