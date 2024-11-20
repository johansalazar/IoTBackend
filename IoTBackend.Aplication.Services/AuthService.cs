using IoTBackend.Aplication.DTOs;
using IoTBackend.Aplication.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IoTBackend.Aplication.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO> AuthenticateAsync(LoginDTO loginDTO)
        {
            // Buscar usuario en base de datos por correo electrónico
            var user = await _userRepository.GetAllUsersAsync();
            var foundUser = user.FirstOrDefault(u => u.Email == loginDTO.Email);

            if (foundUser == null || foundUser.Password != loginDTO.Password) // Aquí deberías verificar la contraseña de manera segura (hashing)
                return null;

            // Crear los claims del usuario
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, foundUser.Name),
                new Claim(ClaimTypes.Email, foundUser.Email),
                new Claim(ClaimTypes.NameIdentifier, foundUser.Id)
            };

            // Obtener la clave secreta desde el archivo de configuración
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Crear el token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Expiración de 1 hora
                signingCredentials: creds
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDTO
            {
                Token = jwtToken,
                Expiration = token.ValidTo
            };
        }
    }
}

