using IoTBackend.Domain.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Aplication.Interfaces
{
    public interface IServidorRepository
    {
        Task<Servidor> GetServidorByIdAsync(string id);
        Task<IEnumerable<Servidor>> GetAllServidoresAsync();
        Task<bool> AddServidorAsync(Servidor servidor);
        Task<bool> UpdateServidorAsync(Servidor servidor);
        Task<bool> DeleteServidorAsync(string id);
    }
}
