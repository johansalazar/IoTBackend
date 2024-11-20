using AutoMapper;
using IoTBackend.Aplication.DTOs;
using IoTBackend.Aplication.Interfaces;
using IoTBackend.Domain.Dominio.Entities;
using IoTBackend.Transversal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Aplication.UseCases
{
    public class LocationUseCases
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationUseCases(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<Response<LocationDTO>> AddLocationAsync(LocationDTO locationDto)
        {
            var response = new Response<LocationDTO>();
            try
            {
                var location = _mapper.Map<Location>(locationDto);
                location.Id = Guid.NewGuid(); // Asignar un nuevo GUID al dispositivo

                var result = await _locationRepository.AddLocationAsync(location);
                if (result)
                {
                    response.Data = _mapper.Map<LocationDTO>(location);
                    response.IsSuccess = true;
                    response.Message = "Ubicación registrada con éxito.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se pudo registrar la ubicación.";
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<LocationDTO>> GetLocationByIdAsync(Guid id)
        {
            var response = new Response<LocationDTO>();
            try
            {
                var location = await _locationRepository.GetLocationByIdAsync(id);
                if (location != null)
                {
                    response.Data = _mapper.Map<LocationDTO>(location);
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Ubicación no encontrada.";
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<IEnumerable<LocationDTO>>> GetAllLocationsAsync()
        {
            var response = new Response<IEnumerable<LocationDTO>>();
            try
            {
                var locations = await _locationRepository.GetAllLocationsAsync();
                response.Data = _mapper.Map<IEnumerable<LocationDTO>>(locations);
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<bool>> DeleteLocationAsync(Guid id)
        {
            var response = new Response<bool>();
            try
            {
                var result = await _locationRepository.DeleteLocationAsync(id);
                response.Data = result;
                response.IsSuccess = result;
                response.Message = result ? "Ubicación eliminada con éxito." : "No se pudo eliminar la ubicación.";
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<bool>> UpdateLocationAsync(LocationDTO locationDTO )
        {
            var response = new Response<bool>();
            try
            {
                var location = _mapper.Map<Location>(locationDTO);
                var result = await _locationRepository.UpdateLocationAsync(location);
                response.Data = result;
                response.IsSuccess = result;
                response.Message = result ? "Location actualizado con éxito." : "No se pudo actualizar el MQTT.";
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
