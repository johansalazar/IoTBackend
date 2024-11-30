namespace IoTBackend.Domain.Dominio.Entities
{
    public class Servidor
    {
        public Guid Id { get; set; }
        public string? NombreHost { get; set; }
        public string? DireccionIP { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? Auditoria { get; set; } // Campo de auditoría (por ejemplo, 'Creado por: Admin, Fecha: 2024-11-20')
        public bool Estado { get; set; }
        public Guid IdMQTT { get; set; }

        // Relación uno a muchos con MQTT
        public MQTT? MQTTs { get; set; } // Un Servidor puede tener muchos MQTT
    }
}
