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
    public class MQTTRepository : IMQTTRepository
    {
        private readonly IoTDbContext _context;

        public MQTTRepository(IoTDbContext context)
        {
            _context = context;
        }

        public async Task<MQTT> GetMQTTByIdAsync(Guid id)
        {
            return await _context.MQTTs.FirstOrDefaultAsync(m => m.Id == id);
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
            _context.MQTTs.Update(mqtt);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteMQTTAsync(Guid id)
        {
            var mqtt = await GetMQTTByIdAsync(id);
            if (mqtt == null) return false;

            _context.MQTTs.Remove(mqtt);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
