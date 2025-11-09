using backend_disc.Repositories;
using AutoMapper;

namespace backend_disc.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto>
    where TEntity : class
    where TDto : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAll();
            return _mapper.Map<List<TDto>>(entities);
        }

        public async Task<TDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetById(id);
            return _mapper.Map<TDto?>(entity);
        }

        public async Task<TDto> CreateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var created = await _repository.Add(entity);
            return _mapper.Map<TDto>(created);
        }

        public async Task<TDto?> UpdateAsync(int id, TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var updated = await _repository.Update(id, entity);
            return _mapper.Map<TDto?>(updated);
        }

        public async Task<TDto?> DeleteAsync(int id)
        {
            var deleted = await _repository.Delete(id);
            return _mapper.Map<TDto?>(deleted);
        }
    }
}
