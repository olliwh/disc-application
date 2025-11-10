using backend_disc.Dtos.BaseDtos;

namespace backend_disc.Dtos.Companies
{
    public class CreatePositionDto : ICreateDtoBase
    {
        public required string Name { get; set; }
    }
}
