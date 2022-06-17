namespace PropMng.Api.Models
{
    public class ChangePasswordDto
    {
        public ChangePasswordDto(EnmLoginStatus status)
        {
            StatusId = (int)status;
        }
        public int StatusId { get; set; }
    }
}
