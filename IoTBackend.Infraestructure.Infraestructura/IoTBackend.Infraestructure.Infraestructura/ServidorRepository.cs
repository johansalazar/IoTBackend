using IoTBackend.Aplication.Interfaces;
using IoTBackend.Domain.Dominio.Entities;
using IoTBackend.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Infraestructure.Infraestructura
{
    public class ServidorRepository : IServidorRepository
    {
        private readonly IoTDbContext _context;

        public ServidorRepository(IoTDbContext context)
        {
            _context = context;
        }

        public async Task<Servidor> GetServidorByIdAsync(Guid id)
        {
            return await _context.Servidores.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Servidor>> GetAllServidoresAsync()
        {
            return await _context.Servidores.ToListAsync();
        }

        public async Task<bool> AddServidorAsync(Servidor servidor)
        {
            _context.Servidores.Add(servidor);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateServidorAsync(Servidor servidor)
        {
            _context.Servidores.Update(servidor);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteServidorAsync(Guid id)
        {
            var servidor = await GetServidorByIdAsync(id);
            if (servidor == null) return false;

            _context.Servidores.Remove(servidor);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
