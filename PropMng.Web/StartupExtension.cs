using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PropMng.Web.Data;

namespace PropMng.Web
{
    public static class StartupExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<AuthenticationStateProvider,  MyAuthenticationStateProvider>();
            services.AddScoped<ValidationHeaderHandler>();
            services.AddHttpClient<IAccountService, AccountService>();
            services.AddHttpClient<ISystemService, SystemService>();
        }

        public static void RegisterBlazoredCompnents(this IServiceCollection services)
        {
            services.AddBlazoredToast();
            services.AddBlazoredLocalStorage();
            services.AddBlazoredModal();
        }
    }
}
