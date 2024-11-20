using IoTBackend.Aplication.Interfaces;
using IoTBackend.Domain.Dominio.Entities;
using IoTBackend.Infraestructure.Persistence.Contexts;
using Microsoft.Azure.Devices;
using Microsoft.EntityFrameworkCore;

namespace IoTBackend.Infraestructure.Infraestructura.IOT
{
    public class IoTDeviceRepository : IIoTDeviceRepository
    {
        private readonly IoTDbContext _context;
        private readonly RegistryManager _registryManager;

        public IoTDeviceRepository(IoTDbContext context, RegistryManager registryManager)
        {
            _context = context;
            _registryManager = registryManager;
        }

        public async Task<IEnumerable<IoTDevice>> GetAllDevicesAsync()
        {
            try
            {
                // Obtiene todos los dispositivos desde la base de datos local
                return await _context.Devices.ToListAsync();
            }
            catch (Exception ex)
            {
                // Manejo de excepciones si ocurre un error con la base de datos
                throw new Exception("Error al obtener los dispositivos", ex);
            }
        }

        public async Task<IoTDevice> GetDeviceByIdAsync(Guid id)
        {
            try
            {
                // Busca el dispositivo en Azure IoT Hub
                var device = await _registryManager.GetDeviceAsync(id.ToString());
                if (device == null)
                {
                    return null; // Retorna null si no existe en Azure IoT Hub
                }

                // Mapear los datos del dispositivo de Azure IoT Hub al modelo IoTDevice
                return new IoTDevice
                {
                    Id = Guid.Parse(device.Id),
                    DeviceKey = device.Authentication.SymmetricKey.PrimaryKey,
                    Estado = device.Status == DeviceStatus.Enabled
                };
            }
            catch (Exception ex)
            {
                // Manejo de excepciones si ocurre un error al obtener el dispositivo de Azure
                throw new Exception("Error al obtener el dispositivo de Azure IoT Hub", ex);
            }
        }

        public async Task<bool> AddDeviceAsync(IoTDevice device)
        {
            try
            {
                // Crear un nuevo dispositivo en Azure IoT Hub
                var newDevice = new Device(device.Id.ToString())
                {
                    Authentication = new AuthenticationMechanism
                    {
                        SymmetricKey = new SymmetricKey
                        {
                            PrimaryKey = device.DeviceKey,
                            SecondaryKey = device.DeviceKey
                        }
                    },
                    Status = device.Estado ? DeviceStatus.Enabled : DeviceStatus.Disabled
                };

                // Añadir el dispositivo a Azure IoT Hub
                var createdDevice = await _registryManager.AddDeviceAsync(newDevice);
                if (createdDevice == null)
                {
                    throw new Exception("No se pudo crear el dispositivo en Azure IoT Hub");
                }

                // Almacena la información del dispositivo en la base de datos local
                _context.Devices.Add(device);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones si ocurre un error en la creación del dispositivo
                throw new Exception("Error al agregar el dispositivo", ex);
            }
        }

        public async Task<bool> DeleteDeviceAsync(Guid id)
        {
            try
            {
                // Elimina el dispositivo de Azure IoT Hub
                await _registryManager.RemoveDeviceAsync(id.ToString());

                // Elimina el dispositivo de la base de datos local
                var device = await _context.Devices.FindAsync(id);
                if (device != null)
                {
                    _context.Devices.Remove(device);
                    return await _context.SaveChangesAsync() > 0;
                }

                return false; // Retorna false si no se encontró el dispositivo en la base de datos
            }
            catch (Exception ex)
            {
                // Manejo de excepciones si ocurre un error al eliminar el dispositivo
                throw new Exception("Error al eliminar el dispositivo", ex);
            }
        }

        public async Task<bool> UpdateDeviceAsync(IoTDevice device)
        {
            try
            {
                // Actualiza el dispositivo en Azure IoT Hub
                var azureDevice = await _registryManager.GetDeviceAsync(device.Id.ToString());
                if (azureDevice != null)
                {
                    azureDevice.Authentication = new AuthenticationMechanism
                    {
                        SymmetricKey = new SymmetricKey
                        {
                            PrimaryKey = device.DeviceKey,
                            SecondaryKey = device.DeviceKey
                        }
                    };
                    azureDevice.Status = device.Estado ? DeviceStatus.Enabled : DeviceStatus.Disabled;

                    await _registryManager.UpdateDeviceAsync(azureDevice);
                }
                else
                {
                    throw new Exception("El dispositivo no existe en Azure IoT Hub.");
                }

                // Actualiza el dispositivo en la base de datos local
                _context.Devices.Update(device);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones si ocurre un error al actualizar el dispositivo
                throw new Exception("Error al actualizar el dispositivo", ex);
            }
        }
    }
}
