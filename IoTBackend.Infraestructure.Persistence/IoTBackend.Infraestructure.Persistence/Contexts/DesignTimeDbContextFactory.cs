using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IoTBackend.Infraestructure.Persistence.Contexts
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IoTDbContext>
    {
        public IoTDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IoTDbContext>();
            optionsBuilder.UseSqlServer("DefaultConnection");  // Reemplaza con tu cadena de conexión

            return new IoTDbContext(optionsBuilder.Options);
        }
    }

}
