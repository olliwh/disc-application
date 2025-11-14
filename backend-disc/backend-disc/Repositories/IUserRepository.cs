
using class_library_disc.Models.Sql;

namespace backend_disc.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> UsernameExists(string username);
    }
}