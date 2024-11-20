using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Domain.Dominio.Entities
{
    public class Location
    {
        public Guid Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Estado { get; set; } // true para activo, false para inactivo
        public DateTime FechaCreacion { get; set; }
    }
}
