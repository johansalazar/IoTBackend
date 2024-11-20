using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Domain.Dominio.Entities
{
    public class IoTDevice
    {
        /// <summary>
        /// Identificador único del dispositivo (puede ser nulo).
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre del dispositivo.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Clave única del dispositivo para autenticación.
        /// </summary>
        public string? DeviceKey { get; set; }

        /// <summary>
        /// Indica si el dispositivo está activo.
        /// </summary>
        public bool Estado { get; set; }

        /// <summary>
        /// Fecha de creación del dispositivo.
        /// </summary>
        public DateTime FechaCreacion { get; set; }
    }
}
