using IoTBackend.Aplication.Interfaces;
using IoTBackend.Domain.Dominio.Entities;
using IoTBackend.Infraestructure.Persistence.Contexts;
using Microsoft.Azure.Devices;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace IoTBackend.Infraestructure.Infraestructura.IOT
{
    public class IoTDeviceRepository : IIoTDeviceRepository
    {
        private readonly IoTDbContext _context;
        private readonly RegistryManager _registryManager;
        private readonly string valueProgram = "TesisJohanSalazar";

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

        public async Task<IoTDevice> GetDeviceByIdAsync(string id)
        {
            try
            {                
                // Busca el dispositivo en Azure IoT Hub
                var device = await _registryManager.GetDeviceAsync(id);
                if (device == null)
                {

                    return null; // Retorna null si no existe en Azure IoT Hub
                }

                //// Decodificar el Base64
                //byte[] originalBytes = Convert.FromBase64String(device.Authentication.SymmetricKey.PrimaryKey);// Decodificar Base64
                //string originalText = Encoding.UTF8.GetString(originalBytes);
                //// Quitar `valueProgram` de `originalText`
                //if (originalText.StartsWith(valueProgram)) // Validar que `valueProgram` está al inicio
                //{
                //    originalText = originalText.Substring(valueProgram.Length); // Remover `valueProgram`
                //}

                var deviceBD = await  _context.Devices.FirstOrDefaultAsync(d => d.Id == Guid.Parse(device.Id));

                IoTDevice ioTDevice = new()
                {
                    Id = deviceBD.Id,
                    Name = deviceBD.Name,
                    DeviceKey = deviceBD.DeviceKey,
                    DeviceType = deviceBD.DeviceType,
                    IdLocation = deviceBD.IdLocation,
                    Estado = deviceBD.Estado,
                    FechaCreacion = deviceBD.FechaCreacion
                };


                // Mapear los datos del dispositivo de Azure IoT Hub al modelo IoTDevice
                return ioTDevice;
                
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
                Guid guid = Guid.NewGuid();
                device.Id = guid;
                device.FechaCreacion = DateTime.Now;


                if (device.DeviceKey == null)
                {
                    throw new ArgumentNullException(nameof(device.DeviceKey), "DeviceKey cannot be null.");
                }

                byte[] decodedBytes = Encoding.UTF8.GetBytes(valueProgram+device.DeviceKey); // Decodificar la cadena Base64
                string FirstKey = Convert.ToBase64String(decodedBytes);          // Volver a codificar en Base64
                string SecondKey = Convert.ToBase64String(decodedBytes);

                var newDevice = new Device(device.Id.ToString())
                {
                    Authentication = new AuthenticationMechanism
                    {
                        SymmetricKey = new SymmetricKey
                        {
                            PrimaryKey = FirstKey,
                            SecondaryKey = SecondKey
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

        public async Task<bool> DeleteDeviceAsync(string id)
        {
            try
            {
                // Elimina el dispositivo de Azure IoT Hub
                await _registryManager.RemoveDeviceAsync(id.ToString());
                return true;                
            }
            catch (Exception ex)
            {
                // Manejo de excepciones si ocurre un error al eliminar el dispositivo
                throw new Exception("Error al eliminar el dispositivo", ex);                
            }
            
        }

        public async Task<bool> DeleteDeviceBdAsync(string id)
        {
            try
            {
                // Elimina el dispositivo de la base de datos local
                var device = await _context.Devices.FindAsync(Guid.Parse(id));
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

        public async Task<bool> UpdateDeviceAsync(string id, IoTDevice device)
        {
            try
            {
                // Actualiza el dispositivo en Azure IoT Hub
                var azureDevice = await _registryManager.GetDeviceAsync(id);
                if (azureDevice == null)
                {
                    throw new Exception("El dispositivo no existe en Azure IoT Hub.");
                }

                if (device.DeviceKey == null)
                {
                    throw new ArgumentNullException(nameof(device.DeviceKey), "DeviceKey cannot be null.");
                }

                // Genera claves de autenticación para el dispositivo
                byte[] decodedBytes = Encoding.UTF8.GetBytes(valueProgram + device.DeviceKey);
                string FirstKey = Convert.ToBase64String(decodedBytes);
                string SecondKey = Convert.ToBase64String(decodedBytes);

                azureDevice.Authentication = new AuthenticationMechanism
                {
                    SymmetricKey = new SymmetricKey
                    {
                        PrimaryKey = FirstKey,
                        SecondaryKey = SecondKey
                    }
                };
                azureDevice.Status = device.Estado ? DeviceStatus.Enabled : DeviceStatus.Disabled;

                // Actualiza el dispositivo en Azure IoT Hub
                await _registryManager.UpdateDeviceAsync(azureDevice);

                // Actualiza el dispositivo en la base de datos local
                var existingDevice = await _context.Devices.FirstOrDefaultAsync(d => d.Id == Guid.Parse(id));
                if (existingDevice == null)
                {
                    throw new Exception("El dispositivo no existe en la base de datos local.");
                }

                // Actualiza las propiedades del dispositivo, excepto 'Id'
                // Si hay más propiedades que actualizar, agrégalas aquí
                existingDevice.Name = device.Name;
                existingDevice.DeviceKey = device.DeviceKey;
                existingDevice.DeviceType = device.DeviceType;
                existingDevice.IdLocation = device.IdLocation;
                existingDevice.Estado = device.Estado;                
                existingDevice.FechaCreacion = DateTime.Now;
                

                // Marca la entidad como modificada
                _context.Devices.Update(existingDevice);

                // Guarda los cambios
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                // Manejo detallado de excepciones
                throw new Exception("Error al actualizar el dispositivo", ex);
            }
        }


    }
}
