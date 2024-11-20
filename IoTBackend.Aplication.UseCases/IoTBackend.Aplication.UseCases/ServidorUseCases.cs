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
    public class ServidorUseCases
    {
        private readonly IServidorRepository _servidorRepository;
        private readonly IMapper _mapper;

        public ServidorUseCases(IServidorRepository servidorRepository, IMapper mapper)
        {
            _servidorRepository = servidorRepository;
            _mapper = mapper;
        }

        public async Task<Response<ServidorDTO>> AddServidorAsync(ServidorDTO servidorDto)
        {
            var response = new Response<ServidorDTO>();
            try
            {
                var servidor = _mapper.Map<Servidor>(servidorDto);
                servidor.Id = Guid.NewGuid(); // Asignar un nuevo GUID al servidor

                // Asegurarse de que el campo de auditoría está presente
                servidor.Auditoria = $"Creado por: Admin, Fecha: {DateTime.UtcNow}";

                var result = await _servidorRepository.AddServidorAsync(servidor);
                if (result)
                {
                    response.Data = _mapper.Map<ServidorDTO>(servidor);
                    response.IsSuccess = true;
                    response.Message = "Servidor registrado con éxito.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se pudo registrar el servidor.";
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<ServidorDTO>> GetServidorByIdAsync(Guid id)
        {
            var response = new Response<ServidorDTO>();
            try
            {
                var servidor = await _servidorRepository.GetServidorByIdAsync(id);
                if (servidor != null)
                {
                    response.Data = _mapper.Map<ServidorDTO>(servidor);
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Servidor no encontrado.";
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<IEnumerable<ServidorDTO>>> GetAllServidoresAsync()
        {
            var response = new Response<IEnumerable<ServidorDTO>>();
            try
            {
                var servidores = await _servidorRepository.GetAllServidoresAsync();
                response.Data = _mapper.Map<IEnumerable<ServidorDTO>>(servidores);
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        public async Task<Response<bool>> DeleteServidorAsync(Guid id)
        {
            var response = new Response<bool>();
            try
            {
                var result = await _servidorRepository.DeleteServidorAsync(id);
                response.Data = result;
                response.IsSuccess = result;
                response.Message = result ? "Servidor eliminado con éxito." : "No se pudo eliminar el servidor.";
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }
        public async Task<Response<bool>> UpdateServidorAsync(ServidorDTO servidorDto)
        {
            var response = new Response<bool>();
            try
            {
                var servidor = _mapper.Map<Servidor>(servidorDto);
                var result = await _servidorRepository.UpdateServidorAsync(servidor);
                response.Data = result;
                response.IsSuccess = result;
                response.Message = result ? "Servidor actualizado con éxito." : "No se pudo actualizar el MQTT.";
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
