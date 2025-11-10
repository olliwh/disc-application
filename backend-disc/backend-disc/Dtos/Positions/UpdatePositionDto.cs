using backend_disc.Dtos.BaseDtos;

namespace backend_disc.Dtos.Companies
{
    public class UpdatePositionDto : IUpdateDtoBase
    {
        public required string Name { get; set; }

    }
}
