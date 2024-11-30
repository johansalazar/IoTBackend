using IoTBackend.Aplication.Interfaces;
using IoTBackend.Domain.Dominio.Entities;
using IoTBackend.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace IoTBackend.Infraestructure.Infraestructura
{
    public class ServidorRepository : IServidorRepository
    {
        private readonly IoTDbContext _context;

        public ServidorRepository(IoTDbContext context)
        {
            _context = context;
        }

        public async Task<Servidor> GetServidorByIdAsync(string id)
        {
            if (Guid.TryParse(id, out Guid parsedId))
            {
                return await _context.Servidores.FirstOrDefaultAsync(s => s.Id == parsedId);
            }
            else
            {
                // Manejar el caso cuando la conversión falle
                throw new ArgumentException("El ID proporcionado no es un GUID válido.", nameof(id));
            }
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
            try
            {
                var existingServidor = await _context.Servidores.FindAsync(servidor.Id);
                if (existingServidor == null)
                    return false;

                _context.Entry(existingServidor).CurrentValues.SetValues(servidor);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteServidorAsync(string id)
        {
            try
            {
                var servidor = await GetServidorByIdAsync(id);
                if (servidor == null) return false;

                // Cambiar el estado a inactivo (false)
                servidor.Estado = false;

                // Marcar la entidad como modificada        

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Aquí puedes registrar el error si es necesario
                Console.WriteLine($"Error al desactivar el servidor: {ex.Message}");
                return false;
            }
        }
    }
}
