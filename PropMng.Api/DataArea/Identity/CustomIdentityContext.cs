using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PropMng.Api.DataArea.Identity
{
    public class CustomIdentityContext : IdentityDbContext<CustomUser>
    {
        public CustomIdentityContext(DbContextOptions<CustomIdentityContext> options) : base(options)
        {

        }

    }
}
