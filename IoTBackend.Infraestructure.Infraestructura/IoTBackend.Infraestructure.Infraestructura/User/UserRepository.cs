using IoTBackend.Aplication.Interfaces;
using IoTBackend.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Infraestructure.Infraestructura.User
{
    public class UserRepository(IoTDbContext context) : IUserRepository
    {
        private readonly IoTDbContext _context = context;

        public async Task<bool> AddUserAsync(Persistence.Contexts.User user)
        {
            try
            {
                Guid guid = Guid.NewGuid();
                user.Id = guid.ToString();
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return false;

                // Cambiar el estado a inactivo (false)
                user.Estado = false;

                // Marcar la entidad como modificada
                _context.Users.Update(user);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Aquí puedes registrar el error si es necesario
                Console.WriteLine($"Error al desactivar el usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<Persistence.Contexts.User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Persistence.Contexts.User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> UpdateUserAsync(Persistence.Contexts.User user)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(user.Id);
                if (existingUser == null)
                    return false;

                _context.Entry(existingUser).CurrentValues.SetValues(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
