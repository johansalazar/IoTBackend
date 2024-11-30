using IoTBackend.Aplication.Interfaces;
using IoTBackend.Domain.Dominio.Entities;
using IoTBackend.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace IoTBackend.Infraestructure.Infraestructura
{
    public class MQTTRepository : IMQTTRepository
    {
        private readonly IoTDbContext _context;

        public MQTTRepository(IoTDbContext context)
        {
            _context = context;
        }

        public async Task<MQTT> GetMQTTByIdAsync(string id)
        {

            if (Guid.TryParse(id, out Guid parsedId))
            {
                return await _context.MQTTs.FirstOrDefaultAsync(m => m.Id == parsedId);
            }
            else
            {
                // Manejar el caso cuando la conversión falle
                throw new ArgumentException("El ID proporcionado no es un GUID válido.", nameof(id));
            }
        }

        public async Task<IEnumerable<MQTT>> GetAllMQTTsAsync()
        {
            return await _context.MQTTs.ToListAsync();
        }

        public async Task<bool> AddMQTTAsync(MQTT mqtt)
        {
            _context.MQTTs.Add(mqtt);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateMQTTAsync(MQTT mqtt)
        {
            try
            {
                var existingMqtt = await _context.MQTTs.FindAsync(mqtt.Id);
                if (existingMqtt == null)
                    return false;

                _context.Entry(existingMqtt).CurrentValues.SetValues(mqtt);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteMQTTAsync(string id)
        {
            try
            {
                var mqtt = await GetMQTTByIdAsync(id);
                if (mqtt == null) return false;

                // Cambiar el estado a inactivo (false)
                mqtt.Estado = false;

                // Marcar la entidad como modificada
                _context.MQTTs.Update(mqtt);

                
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Aquí puedes registrar el error si es necesario
                Console.WriteLine($"Error al desactivar el mqtt: {ex.Message}");
                return false;
            }
        }
    }
}
