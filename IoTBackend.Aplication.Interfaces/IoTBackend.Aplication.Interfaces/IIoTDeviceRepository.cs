using IoTBackend.Domain.Dominio.Entities;

namespace IoTBackend.Aplication.Interfaces
{
    public interface IIoTDeviceRepository
    {
        Task<IoTDevice> GetDeviceByIdAsync(Guid id);
        Task<IEnumerable<IoTDevice>> GetAllDevicesAsync();
        Task<bool> AddDeviceAsync(IoTDevice device);
        Task<bool> UpdateDeviceAsync(IoTDevice device);
        Task<bool> DeleteDeviceAsync(Guid id);
    }
}
