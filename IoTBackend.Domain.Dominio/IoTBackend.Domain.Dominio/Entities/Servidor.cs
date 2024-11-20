using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Domain.Dominio.Entities
{
    public class Servidor
    {
        public Guid Id { get; set; }
        public string? NombreHost { get; set; }
        public string? DireccionIP { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? Auditoria { get; set; } // Campo de auditoría (por ejemplo, 'Creado por: Admin, Fecha: 2024-11-20')
        public bool Estado { get; set; }
    }
}
