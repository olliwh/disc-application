
namespace backend_disc.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> Add(T entity);
        Task<int?> Delete(int id);
        Task<(List<T>, int totalCount)> GetAll(int pageIndex, int pageSize);
        Task<T?> GetById(int id);
        Task<T?> Update(int id, T entity);
    }
}