
using backend_disc.Dtos.BaseDtos;

namespace backend_disc.Services
{
    public interface IGenericService<TDto, TCreateDto, TUpdateDto>
        where TDto : BaseDto
        where TCreateDto : ICreateDtoBase
        where TUpdateDto : IUpdateDtoBase

    {
        Task<List<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(int id);
        Task<TDto> CreateAsync(TCreateDto createDto);
        Task<TDto?> UpdateAsync(int id, TUpdateDto updateDto);
        Task<int?> DeleteAsync(int id);


    }
}