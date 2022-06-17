using Microsoft.AspNetCore.Components;
using PropMng.Api.Models.Models;
using PropMng.Web.Models;

namespace PropMng.Web.Pages
{
    public partial class Index
    {
        [CascadingParameter(Name = "UserRole")] public UserRoleModel UserRole { get; set; }

    }
}
