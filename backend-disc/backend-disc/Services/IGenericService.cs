
using backend_disc.Dtos.BaseDtos;
using backend_disc.Models;

namespace backend_disc.Services
{
    public interface IGenericService<TDto, TCreateDto, TUpdateDto>
        where TDto : BaseDto
        where TCreateDto : ICreateDtoBase
        where TUpdateDto : IUpdateDtoBase

    {
        Task<PaginatedList<TDto>> GetAllAsync(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize);
        Task<TDto?> GetByIdAsync(int id);
        Task<TDto> CreateAsync(TCreateDto createDto);
        Task<TDto?> UpdateAsync(int id, TUpdateDto updateDto);
        Task<int?> DeleteAsync(int id);


    }
}