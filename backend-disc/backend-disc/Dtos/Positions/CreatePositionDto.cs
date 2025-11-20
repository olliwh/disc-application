using backend_disc.Dtos.BaseDtos;

namespace backend_disc.Dtos.Positions

{
    public class CreatePositionDto : ICreateDtoBase
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

    }
}
