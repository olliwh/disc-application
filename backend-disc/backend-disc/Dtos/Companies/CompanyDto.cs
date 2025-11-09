namespace backend_disc.Dtos.Companies
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Location { get; set; }
        public string? BusinessField { get; set; }
    }
}
