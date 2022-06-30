namespace ArdRehber.Dtos
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
        public UserDto UserDto { get; set; }
    }
}
