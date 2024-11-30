namespace IoTBackend.Aplication.DTOs
{
    public class IoTDeviceDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? DeviceKey { get; set; }       
        public string? DeviceType { get; set; }
        public Guid? IdLocation { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
               
    }
}
