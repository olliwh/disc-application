namespace backend_disc.Dtos.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}
