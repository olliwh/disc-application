using AutoMapper;
using backend_disc.Dtos.BaseDtos;
using backend_disc.Factories;
using backend_disc.Models;

namespace backend_disc.Services
{
    public class GenericService<TEntity, TDto, TCreateDto, TUpdateDto> : IGenericService<TDto, TCreateDto, TUpdateDto>
        where TEntity : class
        where TDto : BaseDto
        where TCreateDto : ICreateDtoBase
        where TUpdateDto : IUpdateDtoBase
    {
        private readonly IGenericRepositoryFactory _factory;
        private readonly IMapper _mapper;


        public GenericService(IGenericRepositoryFactory factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TDto>> GetAllAsync(int pageIndex, int pageSize, string db)
        {
            var repo = _factory.GetRepository<TEntity>(db);


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
            var (entities, totalCount) = await repo.GetAll(pageIndex, pageSize);
            var mapped = _mapper.Map<List<TDto>>(entities);
            return new PaginatedList<TDto>(mapped, pageIndex, totalCount, pageSize);
        }


        public async Task<TDto?> GetByIdAsync(int id, string db)
        {
            var repo = _factory.GetRepository<TEntity>(db);

            var entity = await repo.GetById(id);
            return _mapper.Map<TDto?>(entity);
        }

        public virtual async Task<TDto> CreateAsync(TCreateDto createDto, string db)
        {
            try
            {
                var repo = _factory.GetRepository<TEntity>(db);

                var entity = _mapper.Map<TEntity>(createDto);
                var created = await repo.Add(entity);
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

        public async Task<int?> DeleteAsync(int id, string db)
        {
            var repo = _factory.GetRepository<TEntity>(db);

            var deleted = await repo.Delete(id);
            return deleted;
        }

        public async Task<TDto?> UpdateAsync(int id, TUpdateDto updateDto, string db)
        {
            try
            {
                var repo = _factory.GetRepository<TEntity>(db);

                var entity = _mapper.Map<TEntity>(updateDto);
                var updated = await repo.Update(id, entity);
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
