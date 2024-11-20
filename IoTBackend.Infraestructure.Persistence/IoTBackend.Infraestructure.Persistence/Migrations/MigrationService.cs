using IoTBackend.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Infraestructure.Persistence.Migrations
{
    public class MigrationService
    {
        private readonly IoTDbContext _context;
        private readonly IMigrator _migrator;

        public MigrationService(IoTDbContext context, IMigrator migrator)
        {
            _context = context;
            _migrator = migrator;
        }

        public async Task MigrateDatabaseAsync()
        {
            try
            {
                // Ejecutar migraciones si hay cambios pendientes
                await _migrator.MigrateAsync();
            }
            catch (Exception ex)
            {
                // Manejo de errores (si es necesario)
                Console.WriteLine($"Error al ejecutar migraciones: {ex.Message}");
            }
        }
    }
}
