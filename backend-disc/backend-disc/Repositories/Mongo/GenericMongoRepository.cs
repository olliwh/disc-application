
namespace backend_disc.Repositories.Mongo
{
    public class GenericMongoRepository<T> : IGenericRepository<T> where T : class
    {
        public Task<T> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<(List<T>, int totalCount)> GetAll(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<T?> Update(int id, T entity)
        {
            throw new NotImplementedException();
        }
    }
}
