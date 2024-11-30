using IoTBackend.Domain.Dominio.Entities;
using Microsoft.EntityFrameworkCore;

namespace IoTBackend.Infraestructure.Persistence.Contexts
{
    /// <summary>
    /// Contexto principal para la base de datos de IoT, maneja la persistencia de las entidades 
    /// del dominio como usuarios, dispositivos IoT, servidores, ubicaciones y configuraciones MQTT.
    /// </summary>
    public class IoTDbContext : DbContext
    {
        /// <summary>
        /// Constructor del contexto que recibe las opciones de configuración de DbContext.
        /// </summary>
        /// <param name="options">Opciones de configuración para el DbContext</param>
        public IoTDbContext(DbContextOptions<IoTDbContext> options) : base(options) { }

        // Definición de las tablas para las entidades del dominio
        public DbSet<User>? Users { get; set; }
        public DbSet<Location>? Locations { get; set; }
        public DbSet<Servidor>? Servidores { get; set; }
        public DbSet<MQTT>? MQTTs { get; set; }
        public DbSet<IoTDevice>? Devices { get; set; }

        /// <summary>
        /// Método para configurar las entidades y sus relaciones en la base de datos.
        /// </summary>
        /// <param name="modelBuilder">Objeto utilizado para configurar el modelo de la base de datos.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para la tabla Location
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.Property(l => l.Id)
                      .HasMaxLength(36)
                      .IsRequired(true);

                entity.Property(l => l.Nombre)
                      .HasMaxLength(100)
                      .IsRequired(false);

                entity.Property(l => l.Descripcion)
                      .HasMaxLength(150)
                      .IsRequired();

                entity.Property(l => l.Estado);
                entity.Property(l => l.FechaCreacion);

                // Configuración de la relación con IoTDevice (uno a muchos)
                entity.HasMany(l => l.IoTDevices)
                      .WithOne(d => d.Location) // Relación inversa con IoTDevice
                      .HasForeignKey(d => d.Id) // Clave foránea en IoTDevice
                      .OnDelete(DeleteBehavior.SetNull); // Eliminar la relación, no el dispositivo

                entity.HasOne(l => l.Servidor) // Un Location está asociado a un único Servidor
                        .WithMany() // Un Servidor tiene  Location
                        .HasForeignKey(l => l.ServidorId) // Clave foránea en Location
                        .OnDelete(DeleteBehavior.Cascade); // Cascada al eliminar el Servidor

            });

            // Configuración para la tabla IoTDevice
            modelBuilder.Entity<IoTDevice>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Id)
                      .HasMaxLength(36)
                      .IsRequired(true);

                entity.Property(d => d.Name)
                      .HasMaxLength(100)
                      .IsRequired(false);

                entity.Property(d => d.DeviceKey)
                      .HasMaxLength(50)
                      .IsRequired(false);

                entity.Property(d => d.DeviceType)
                      .HasMaxLength(50)
                      .IsRequired(false);

                entity.Property(d => d.IdLocation)
                      .HasMaxLength(36)
                      .IsRequired(true);

                entity.Property(d => d.Estado);
                entity.Property(d => d.FechaCreacion);

                // Relación con Location (uno a muchos)
                entity.HasOne(d => d.Location)
                       .WithMany(l => l.IoTDevices)
                       .HasForeignKey(d => d.IdLocation)
                       .OnDelete(DeleteBehavior.Cascade); // Eliminar la relación si se elimina Location
            });

            modelBuilder.Entity<Servidor>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Id)
                      .IsRequired()
                      .HasMaxLength(36);

                entity.Property(s => s.NombreHost)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(s => s.DireccionIP)
                      .HasMaxLength(150);

                entity.Property(s => s.Estado)
                      .IsRequired();

                entity.Property(s => s.FechaCreacion)
                      .IsRequired();

                entity.Property(s => s.Auditoria)
                      .HasMaxLength(150);

                // Relación uno a uno con MQTT
                entity.HasOne(s => s.MQTTs) // Un Servidor tiene un MQTT
                      .WithMany()          // No hay navegación desde MQTT hacia Servidor
                      .HasForeignKey(s => s.IdMQTT) // IdMQTT es la clave foránea
                      .OnDelete(DeleteBehavior.Restrict); // No se elimina el MQTT al eliminar el Servidor
            });

            modelBuilder.Entity<MQTT>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Id)
                      .IsRequired()
                      .HasMaxLength(36);

                entity.Property(m => m.DireccionIPBroker)
                      .HasMaxLength(150);

                entity.Property(m => m.UsuarioMQTT)
                      .HasMaxLength(100);

                entity.Property(m => m.ClaveMQTT)
                      .HasMaxLength(100);

                entity.Property(m => m.Estado)
                      .IsRequired();

                entity.Property(m => m.FechaCreacion)
                      .IsRequired();

                entity.Property(m => m.Auditoria)
                      .HasMaxLength(150);
            });
        }
    }
}
