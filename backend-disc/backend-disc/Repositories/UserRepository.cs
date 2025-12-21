using class_library_disc.Data;
using class_library_disc.Models.Sql;
using Microsoft.EntityFrameworkCore;

namespace backend_disc.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DiscProfileDbContext _context;

        public UserRepository(DiscProfileDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users
                .Include(u => u.Employee)
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }
    }
}
