using IoTBackend.Domain.Dominio.Entities;
using Microsoft.EntityFrameworkCore;

namespace IoTBackend.Infraestructure.Persistence.Contexts
{
    public class IoTDbContext(DbContextOptions<IoTDbContext> options) : DbContext(options)
    {
        public DbSet<IoTDevice>? Devices { get; set; }
        public DbSet<User>? Users { get; set; } // Tabla de usuarios
        public DbSet<Location>? Locations { get; set; } // Tabla de Locations
        public DbSet<Servidor>? Servidores { get; set; } // Tabla de Servidor
        public DbSet<MQTT>? MQTTs { get; set; } // Tabla de Servidor       


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IoTDevice>(static entity =>
            {
                // Configuración de la clave primaria
                entity.HasKey(d => d.Id);

                // Configuración de propiedades
                entity.Property(d => d.Id)
                      .HasMaxLength(36) // Longitud típica para UUID en formato string
                      .IsRequired(true);

                entity.Property(d => d.Name)
                      .HasMaxLength(100)
                      .IsRequired(false);

                entity.Property(d => d.DeviceKey)
                      .HasMaxLength(50)
                      .IsRequired(false);

                entity.Property(d => d.Estado);

                entity.Property(d => d.FechaCreacion);

            });

            // Configuración para la tabla User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                      .HasMaxLength(36)
                      .IsRequired(true);

                entity.Property(u => u.Name)
                      .HasMaxLength(100)
                      .IsRequired(false);

                entity.Property(u => u.Email)
                      .HasMaxLength(150)
                      .IsRequired();

                entity.Property(u => u.Password)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(u => u.Estado);

                entity.Property(u => u.FechaCreacion);
            });

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
            });

            // Configuración para la tabla Location
            modelBuilder.Entity<MQTT>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Id)
                      .HasMaxLength(36)
                      .IsRequired(true);

                entity.Property(m => m.DireccionIPBroker)
                      .HasMaxLength(150)
                      .IsRequired(false);

                entity.Property(m => m.UsuarioMQTT)
                      .HasMaxLength(150)
                      .IsRequired();

                entity.Property(m => m.ClaveMQTT)
                      .HasMaxLength(150)
                      .IsRequired();

                entity.Property(m => m.Estado);

                entity.Property(m => m.Auditoria)
                      .HasMaxLength(150)
                      .IsRequired();
                entity.Property(m => m.FechaCreacion);
            });

        }
    }
}
