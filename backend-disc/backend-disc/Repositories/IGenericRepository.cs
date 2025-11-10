
namespace backend_disc.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Add(T entity);
        IQueryable<T> Query();
        Task<int?> Delete(int id);
        Task<List<T>> GetAll();
        Task<T?> GetById(int id);
        Task<T?> Update(int id, T entity);
    }
}