using Asp.Versioning;
using IoTBackend.Aplication.DTOs;
using IoTBackend.Aplication.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoTBackend.Services.WebAppi.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class UsersController(UserUseCases userUseCases) : ControllerBase
    {
        private readonly UserUseCases _userUseCases = userUseCases;

        /// <summary>
        /// Método para crear un nuevo usuario.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserDTO userDto)
        {
            if (userDto == null)
                return BadRequest("El usuario no puede ser nulo.");

            var response = await _userUseCases.AddUserAsync(userDto);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Método para eliminar un usuario por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteUser/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var response = await _userUseCases.DeleteUserAsync(id);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Método para obtener todos los usuarios.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllUsers")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userUseCases.GetAllUsersAsync();
            return Ok(response);
        }

        /// <summary>
        /// Método para obtener un usuario por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUserById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string id)
        {
            var response = await _userUseCases.GetUserByIdAsync(id);
            if (response == null)
                return NotFound("Usuario no encontrado.");

            return Ok(response);
        }

        /// <summary>
        /// Método para actualizar un usuario existente.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateUser/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDTO userDto)
        {
            if (userDto == null)
                return BadRequest("El usuario no puede ser nulo.");
            userDto.Id = id;
            var response = await _userUseCases.UpdateUserAsync(id, userDto);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }
    }
}
