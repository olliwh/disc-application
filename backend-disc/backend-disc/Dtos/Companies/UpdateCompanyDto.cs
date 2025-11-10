using backend_disc.Dtos.BaseDtos;

namespace backend_disc.Dtos.Companies
{
    public class UpdateCompanyDto : IUpdateDtoBase
    {
        public required string Name { get; set; }
        public string? Location { get; set; }
        public string? BusinessField { get; set; }
    }
}
