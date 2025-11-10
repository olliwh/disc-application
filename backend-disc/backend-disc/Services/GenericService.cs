using backend_disc.Repositories;
using AutoMapper;
using backend_disc.Dtos.BaseDtos;

namespace backend_disc.Services
{
    public class GenericService<TEntity, TDto, TCreateDto, TUpdateDto> : IGenericService<TDto, TCreateDto, TUpdateDto>
        where TEntity : class
        where TDto : BaseDto
        where TCreateDto : ICreateDtoBase
        where TUpdateDto : IUpdateDtoBase
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

        public virtual async Task<TDto> CreateAsync(TCreateDto createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            var created = await _repository.Add(entity);
            return _mapper.Map<TDto>(created);
        }

        public async Task<int?> DeleteAsync(int id)
        {
            var deleted = await _repository.Delete(id);
            return deleted;
        }

        public async Task<TDto?> UpdateAsync(int id, TUpdateDto updateDto)
        {
            var entity = _mapper.Map<TEntity>(updateDto);
            var updated = await _repository.Update(id, entity);
            return _mapper.Map<TDto?>(updated); 
        }
    }
}
