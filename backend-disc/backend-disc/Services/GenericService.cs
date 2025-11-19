using AutoMapper;
using backend_disc.Dtos.BaseDtos;
using backend_disc.Models;
using backend_disc.Repositories;

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

        public async Task<PaginatedList<TDto>> GetAllAsync(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageSize > 50)
            {
                pageSize = 50; // max page size
            }
            var entities = await _repository.GetAll();
            var mapped = _mapper.Map<List<TDto>>(entities);
            return new PaginatedList<TDto>(mapped, pageIndex, entities.Count, pageSize);
        }

        public async Task<List<TDto>> GetAllAsync2()
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
            try
            {
                var entity = _mapper.Map<TEntity>(createDto);
                var created = await _repository.Add(entity);
                return _mapper.Map<TDto>(created);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while creating the entity", ex);
            }
        }

        public async Task<int?> DeleteAsync(int id)
        {
            var deleted = await _repository.Delete(id);
            return deleted;
        }

        public async Task<TDto?> UpdateAsync(int id, TUpdateDto updateDto)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(updateDto);
                var updated = await _repository.Update(id, entity);
                return _mapper.Map<TDto?>(updated);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while creating the entity", ex);
            }
        }
    }
}
