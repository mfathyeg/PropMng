using Microsoft.AspNetCore.Components;
using PropMng.Api.Models.Models;

namespace PropMng.Web.Shared
{
    public partial class NavMenu
    {
        [CascadingParameter(Name = "UserRole")] public UserRoleModel UserRole { get; set; }

    }
}
