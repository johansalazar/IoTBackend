using AutoMapper;
using IoTBackend.Aplication.DTOs;
using IoTBackend.Aplication.Interfaces;
using IoTBackend.Infraestructure.Persistence.Contexts;
using IoTBackend.Transversal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Aplication.UseCases
{
    public class UserUseCases(IUserRepository userRepository, IMapper mapper)
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        // Agregar un nuevo usuario
        public async Task<Response<UserDTO>> AddUserAsync(UserDTO userDto)
        {
            var response = new Response<UserDTO>();
            try
            {
                // Mapeo del DTO a modelo de dominio
                var user = _mapper.Map<User>(userDto);

                // Agregar el usuario en la base de datos
                var result = await _userRepository.AddUserAsync(user);
                if (result)
                {
                    response.Data = userDto;
                    response.IsSuccess = true;
                    response.Message = "Usuario creado exitosamente.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se pudo crear el usuario.";
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        // Obtener un usuario por ID
        public async Task<UserDTO> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user != null ? _mapper.Map<UserDTO>(user) : null;
        }

        // Obtener todos los usuarios
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        // Actualizar un usuario
        public async Task<Response<UserDTO>> UpdateUserAsync(Guid id, UserDTO userDto)
        {
            var response = new Response<UserDTO>();
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Usuario no encontrado.";
                    return response;
                }

                // Mapeo del DTO al modelo de dominio
                _mapper.Map(userDto, user);

                var result = await _userRepository.UpdateUserAsync(user);
                if (result)
                {
                    response.Data = userDto;
                    response.IsSuccess = true;
                    response.Message = "Usuario actualizado exitosamente.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se pudo actualizar el usuario.";
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = $"Error: {e.Message}";
            }

            return response;
        }

        // Eliminar un usuario
        public async Task<Response<UserDTO>> DeleteUserAsync(Guid id)
        {
            var response = new Response<UserDTO>();
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Usuario no encontrado.";
                    return response;
                }

                var result = await _userRepository.DeleteUserAsync(id);
                if (result)
                {
                    response.IsSuccess = true;
                    response.Message = "Usuario eliminado exitosamente.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se pudo eliminar el usuario.";
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
