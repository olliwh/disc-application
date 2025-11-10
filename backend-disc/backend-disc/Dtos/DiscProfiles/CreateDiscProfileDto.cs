using backend_disc.Dtos.BaseDtos;

namespace backend_disc.Dtos.Companies
{
    public class CreateDiscProfileDto : ICreateDtoBase
    {
        public required string Name { get; set; }
        public required string Color { get; set; }
        public required string Description { get; set; }
    }
}
