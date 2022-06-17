namespace PropMng.Api.Models.Models
{
    public class LoginBaseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int StatusId { get; set; }
    }
}
