using Microsoft.AspNetCore.Identity;

namespace PropMng.Api.DataArea.Identity
{
    public class CustomUser : IdentityUser
    {
        public long? PersonId { get; set; }
    }
}