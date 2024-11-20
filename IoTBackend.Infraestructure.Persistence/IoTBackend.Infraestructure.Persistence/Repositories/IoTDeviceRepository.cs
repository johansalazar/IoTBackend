using IoTBackend.Aplication.Interfaces;
using IoTBackend.Domain.Dominio.Entities;
using IoTBackend.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace IoTBackend.Infraestructure.Persistence.Repositories
{
    public class IoTDeviceRepository(IoTDbContext context) : IIoTDeviceRepository
    {
        public async Task<bool> AddDeviceAsync(IoTDevice device)
        {
            await context.Devices.AddAsync(device);
            return await context.SaveChangesAsync() > 0; // Retorna true si se guardó exitosamente
        }
        public async Task<bool> DeleteDeviceAsync(Guid id)
        {
            var device = await context.Devices.FindAsync(id);
            if (device != null)
            {
                context.Devices.Remove(device);
                return await context.SaveChangesAsync() > 0; // Retorna true si se eliminó exitosamente
            }
            return false; // Retorna false si no se encontró el dispositivo
        }
        public async Task<IEnumerable<IoTDevice>> GetAllDevicesAsync()
        {
            return await context.Devices.ToListAsync(); // Retorna la lista completa de dispositivos
        }

        public async Task<IoTDevice> GetDeviceByIdAsync(Guid id)
        {
            return await context.Devices.FindAsync(id); // Retorna el dispositivo encontrado o null
        }

        public async Task<bool> UpdateDeviceAsync(IoTDevice device)
        {
            context.Devices.Update(device);
            return await context.SaveChangesAsync() > 0; // Retorna true si se actualizó exitosamente

        }
    }
}
