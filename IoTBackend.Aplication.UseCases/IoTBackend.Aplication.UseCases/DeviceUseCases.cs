using AutoMapper;
using IoTBackend.Aplication.DTOs;
using IoTBackend.Aplication.Interfaces;
using IoTBackend.Domain.Dominio.Entities;
using IoTBackend.Transversal.Common;

namespace IoTBackend.Aplication.UseCases
{
    public class DeviceUseCases(IIoTDeviceRepository repository, IMapper mapper)
    {
        public async Task<Response<IoTDevice>> AddDeviceAsync(IoTDeviceDTO devicedto)
        {
            var response = new Response<IoTDevice>();
            try
            {
                var device = mapper.Map<IoTDevice>(devicedto);
                var result = await repository.AddDeviceAsync(device);
                if (result)
                {
                    response.Data = device;
                    response.IsSuccess = true;
                    response.Message = "Registro Exitoso!!!";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se pudo registrar el dispositivo.";
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<IoTDevice>> GetDeviceByIdAsync(Guid id)
        {
            var response = new Response<IoTDevice>();
            try
            {
                var device = await repository.GetDeviceByIdAsync(id);
                if (device != null)
                {
                    response.Data = device;
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Dispositivo no encontrado.";
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<IEnumerable<IoTDevice>>> GetAllDevicesAsync()
        {
            var response = new Response<IEnumerable<IoTDevice>>();
            try
            {
                var devices = await repository.GetAllDevicesAsync();
                response.Data = devices;
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<IoTDevice>> UpdateDeviceAsync(Guid id, IoTDeviceDTO devicedto)
        {
            var response = new Response<IoTDevice>();
            try
            {
                var device = await repository.GetDeviceByIdAsync(id);
                if (device == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Dispositivo no encontrado.";
                    return response;
                }

                // Actualiza los datos del dispositivo
                mapper.Map(devicedto, device);

                var result = await repository.UpdateDeviceAsync(device);
                if (result)
                {
                    response.Data = device;
                    response.IsSuccess = true;
                    response.Message = "Actualización exitosa.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se pudo actualizar el dispositivo.";
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<bool>> DeleteDeviceAsync(Guid id)
        {
            var response = new Response<bool>();
            try
            {
                var device = await repository.GetDeviceByIdAsync(id);
                if (device == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Dispositivo no encontrado.";
                    return response;
                }

                var result = await repository.DeleteDeviceAsync(id);
                if (result)
                {
                    response.Data = true;
                    response.IsSuccess = true;
                    response.Message = "Eliminación exitosa.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se pudo eliminar el dispositivo.";
                }
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
