using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using Blazored.Modal.Services;
using Blazored.Modal;
using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Web;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Http;
using Blazored.Toast.Services;
using PropMng.Web.Data;
using PropMng.Web.Models;
using PropMng.Api.Models.Models;
using PropMng.Api.Models;

namespace PropMng.Web.Pages
{
    public partial class Login
    {
        [Inject] IJSRuntime JsRuntime { get; set; }
        [Parameter] public string Portal { get; set; }
        [Inject] AuthenticationStateProvider StateProvider { get; set; }
        [Inject] IConfiguration Configuration { get; set; }
        [Inject] NavigationManager NavManager { get; set; }
        [CascadingParameter] public IModalService ModalService { get; set; }
        [Inject] IAccountService AccountService { get; set; }
        [Inject] ISystemService SystemService { get; set; }
        [Inject] IToastService ToastService { get; set; }

        private bool IsLoading { get; set; } = false;

        private LoginBaseModel loginModel { get; set; }
        [Inject] IHttpContextAccessor httpContextAccessor { get; set; }



        protected override async Task OnInitializedAsync()
        {
            loginModel = new LoginBaseModel();
            await ((MyAuthenticationStateProvider)StateProvider).LogOutUser();
            await FillBrowserIpAddressDetails();

            await base.OnInitializedAsync();
        }

        private async Task FillBrowserIpAddressDetails()
        {
            var ipData = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            loginModel.IpAddress = ipData;
        }

        private async Task ValidateUser()
        {
            IsLoading = true;
            if (!(string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password)))
            {
                var user = await AccountService.LoginAsync(loginModel);
                if (user == null)
                {
                    ShowError("AN ERROR OCCURED WHILE CONNECTING TO DATABASE");
                    IsLoading = false;
                    return;
                }
                else if (user.StatusId == (int)EnmLoginStatus.Success)
                {
                    await ((MyAuthenticationStateProvider)StateProvider).LogInUser(new UserSessionModel { UserName = loginModel.Username, AccessToken = user.AccessToken, RefreshToken = user.RefreshToken });
                    NavManager.NavigateTo("/", true);
                }
                else if (user.StatusId == (int)EnmLoginStatus.Failed)
                {
                    ShowError("ERROR IN USERNAME OR PASSWORD");
                }
                else if (user.StatusId == (int)EnmLoginStatus.LockedOut)
                {
                    ShowError("YOUR ACCOUNT IS LOCKED.");
                }
            }
            else
            {
                ShowError("رجاء إدخال إسم المستخدم وكلمة المرور");
            }
            IsLoading = false;
        }
        private bool showMessage = false;
        private string ErrorMessage = string.Empty;
        private void ShowError(string msg)
        {
            showMessage = true;
            ErrorMessage = msg;

        }
    }
}
