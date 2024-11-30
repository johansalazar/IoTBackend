using IoTBackend.Domain.Dominio.Entities;

namespace IoTBackend.Aplication.Interfaces
{
    public interface IIoTDeviceRepository
    {
        Task<IoTDevice> GetDeviceByIdAsync(string id);
        Task<IEnumerable<IoTDevice>> GetAllDevicesAsync();
        Task<bool> AddDeviceAsync(IoTDevice device);
        Task<bool> UpdateDeviceAsync(string id,IoTDevice device);
        Task<bool> DeleteDeviceAsync(string id);
        Task<bool> DeleteDeviceBdAsync(string id);
    }
}
