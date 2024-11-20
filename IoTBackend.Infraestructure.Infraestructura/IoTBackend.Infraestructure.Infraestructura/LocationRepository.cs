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
    public class LocationRepository : ILocationRepository
    {
        private readonly IoTDbContext _context;

        public LocationRepository(IoTDbContext context)
        {
            _context = context;
        }

        public async Task<Location> GetLocationByIdAsync(Guid id)
        {
            return await _context.Locations.FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _context.Locations.ToListAsync();
        }

        public async Task<bool> AddLocationAsync(Location location)
        {
            _context.Locations.Add(location);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateLocationAsync(Location location)
        {
            _context.Locations.Update(location);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteLocationAsync(Guid id)
        {
            var location = await GetLocationByIdAsync(id);
            if (location == null) return false;

            _context.Locations.Remove(location);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
