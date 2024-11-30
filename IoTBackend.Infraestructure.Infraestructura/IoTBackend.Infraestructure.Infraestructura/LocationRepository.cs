using IoTBackend.Aplication.Interfaces;
using IoTBackend.Domain.Dominio.Entities;
using IoTBackend.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace IoTBackend.Infraestructure.Infraestructura
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IoTDbContext _context;

        public LocationRepository(IoTDbContext context)
        {
            _context = context;
        }

        public async Task<Location> GetLocationByIdAsync(string id)
        {
            if (Guid.TryParse(id, out Guid parsedId))
            {
                return await _context.Locations.FirstOrDefaultAsync(l => l.Id == parsedId);
            }
            else
            {
                // Manejar el caso cuando la conversión falle
                throw new ArgumentException("El ID proporcionado no es un GUID válido.", nameof(id));
            }
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _context.Locations.ToListAsync();
        }

        public async Task<bool> AddLocationAsync(Location location)
        {
            Guid guid = Guid.NewGuid();
            location.Id = guid;
            _context.Locations.Add(location);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateLocationAsync(Location location)
        {
            try
            {
                var existingLocation = await _context.Locations.FindAsync(location.Id);
                if (existingLocation == null)
                    return false;

                _context.Entry(existingLocation).CurrentValues.SetValues(location);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteLocationAsync(string id)
        {
            try
            {
                var location = await GetLocationByIdAsync(id);
                if (location == null) return false;

                // Cambiar el estado a inactivo (false)
                location.Estado = false;

                // Marcar la entidad como modificada
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Aquí puedes registrar el error si es necesario
                Console.WriteLine($"Error al desactivar el location: {ex.Message}");
                return false;
            }
        }
    }
}
