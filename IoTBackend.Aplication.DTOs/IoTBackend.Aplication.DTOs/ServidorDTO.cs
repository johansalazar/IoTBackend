namespace IoTBackend.Aplication.DTOs
{
    public class ServidorDTO
    {
        public Guid Id { get; set; }
        public string? NombreHost { get; set; }
        public string? DireccionIP { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? Auditoria { get; set; }
        public bool Estado { get; set; }
        public Guid IdMQTT { get; set; }            
      
    }
}
