using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PropMng.Web.Data
{
    public class MyAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;

        public MyAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await localStorageService.GetItemAsync<UserSessionModel>("UserSession");
                var identity = new ClaimsIdentity();
                if (userSession != null && !string.IsNullOrEmpty(userSession.UserName))
                {
                    identity = new ClaimsIdentity(new[]
                  {
                    new Claim(ClaimTypes.Name,userSession.UserName)
                }, "apiauth_Type");
                }
                var user = new ClaimsPrincipal(identity);
                return await Task.FromResult(new AuthenticationState(user));
            }
            catch (System.Exception ex)
            {
                return null;
            }

        }

        public async Task LogInUser(UserSessionModel userSession)
        {
            await localStorageService.SetItemAsync("UserSession", userSession);
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name,userSession.UserName)
            }, "apiauth_Type");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task LogOutUser()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            await localStorageService.RemoveItemAsync("UserSession");
        }
    }
}
