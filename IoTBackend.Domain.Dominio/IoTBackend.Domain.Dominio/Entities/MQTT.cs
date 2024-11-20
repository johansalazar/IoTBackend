using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Domain.Dominio.Entities
{
    public class MQTT
    {
        public Guid Id { get; set; }
        public string? DireccionIPBroker { get; set; }
        public string? UsuarioMQTT { get; set; }
        public string? ClaveMQTT { get; set; }
        public bool Estado { get; set; }
        public string? Auditoria { get; set; } // Auditoría (por ejemplo, 'Creado por: Admin, Fecha: 2024-11-20')
        public DateTime FechaCreacion { get; set; }
    }
}
