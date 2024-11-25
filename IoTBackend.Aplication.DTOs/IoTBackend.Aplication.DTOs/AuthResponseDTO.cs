using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Aplication.DTOs
{
    public class AuthResponseDTO
    {
        public string? Token { get; set; }
        public string? Name { get; set; }
        public DateTime Expiration { get; set; }
    }
}
