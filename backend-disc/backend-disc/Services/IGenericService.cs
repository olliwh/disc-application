
namespace backend_disc.Services
{
    public interface IGenericService<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        Task<List<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(int id);
        Task<TDto> CreateAsync(TDto dto);
        Task<TDto?> UpdateAsync(int id, TDto dto);
        Task<TDto?> DeleteAsync(int id);
    }
}