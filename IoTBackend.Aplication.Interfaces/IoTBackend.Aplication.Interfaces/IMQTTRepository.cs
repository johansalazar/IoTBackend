using IoTBackend.Domain.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Aplication.Interfaces
{
    public interface IMQTTRepository
    {
        Task<MQTT> GetMQTTByIdAsync(string id);
        Task<IEnumerable<MQTT>> GetAllMQTTsAsync();
        Task<bool> AddMQTTAsync(MQTT mqtt);
        Task<bool> UpdateMQTTAsync(MQTT mqtt);
        Task<bool> DeleteMQTTAsync(string id);
    }
}
