using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Aplication.DTOs
{
    public class UserDTO
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Contraseña del usuario (debe ser cifrada).
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Indica si el usuario está activo.
        /// </summary>
        public bool Estado { get; set; }

        /// <summary>
        /// Fecha de creación del usuario.
        /// </summary>
        public DateTime FechaCreacion { get; set; }
    }
}
