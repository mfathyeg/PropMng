using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using PropMng.Api.Models.Models;
using PropMng.Web.Data;
using PropMng.Web.Models;
using System.Net;
using System.Threading.Tasks;

namespace PropMng.Web.Shared
{
    public partial class MainLayout
    {
        [CascadingParameter] Task<AuthenticationState> AuthProvider { get; set; }

        [Inject] NavigationManager NavManager { get; set; }

        [Inject] IConfiguration Configuration { get; set; }

        [Inject] ISystemService SystemService { get; set; }

        private ProfileModel myProfile = new ProfileModel();
        private UserRoleModel userRole = new UserRoleModel();

        private async Task LoadMyProfileModel()
        {
            var obj = await SystemService.GetAsync("System/Profile/Details");
            myProfile = obj.StatusCode == HttpStatusCode.OK && obj.IsSucceed
                ? obj.GetDsrlzT<ProfileModel>()
                : new ProfileModel();
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadMyProfileModel();
            await CheckPageAuthentication();
        }

        private async Task CheckPageAuthentication()
        {
            var pageName = GetPageName(NavManager.ToBaseRelativePath(NavManager.Uri).ToLower()).ToLower();
            if (pageName == "login") return;

            if (!(await AuthProvider).User.Identity.IsAuthenticated)
            {
                NavManager.NavigateTo("/Login");
            }
            else
            {
                var resp = await SystemService.GetAsync($"System/Account/UserRole",true);
                if(resp.IsSucceed)
                    userRole = resp.GetDsrlzT<UserRoleModel>(); 
            }
        }

        private string GetPageName(string url)
        {
            if (url.Contains("?"))
                url = url.Split('?')[0];
            if (url == "index" || url == string.Empty)
                return string.Empty;

            var urlItems = url.Split('/');
            if (urlItems.Length == 1)
                return urlItems[0];

            var areas = Configuration["SystemSettings:AreasNames"];
            if (string.IsNullOrEmpty(areas))
                return urlItems[0];

            var areasArr = areas.Split(",");
            foreach (var i in areasArr)
                if (urlItems[0] == i)
                    return urlItems[1];
            return urlItems[0];
        }
    }
}
