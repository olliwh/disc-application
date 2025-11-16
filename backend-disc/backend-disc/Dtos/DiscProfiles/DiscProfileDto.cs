namespace backend_disc.Dtos.DiscProfiles
{
    public class DiscProfileDto : BaseDtos.BaseDto
    {
        public required string Name { get; set; }
        public required string Color { get; set; }
        public required string Description { get; set; }
    }
}
