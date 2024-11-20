using IoTBackend.Domain.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Aplication.Interfaces
{
    public interface ILocationRepository
    {
        Task<Location> GetLocationByIdAsync(Guid id);
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<bool> AddLocationAsync(Location location);
        Task<bool> UpdateLocationAsync(Location location);
        Task<bool> DeleteLocationAsync(Guid id);
    }
}
