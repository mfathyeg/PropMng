using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Blazored.LocalStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using Blazored.Toast.Services;
using PropMng.Web.Models;

namespace PropMng.Web.Data
{

    public class SystemService : ISystemService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;
        private readonly IAccountService accountService;
        private readonly IToastService toastService;

        public SystemService(IConfiguration configuration, HttpClient httpClient, ILocalStorageService localStorage, IAccountService accountService, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
            this.accountService = accountService;
            this.toastService=toastService;
            httpClient.BaseAddress = new Uri(configuration["ServicesBaseAddresses:ServicesAddress"]);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "BlozerServer");
        }
        public async Task<PostRespModel> GetAsync(string requestUri, object model, bool showErrorMsg)
        {
            if (model!=null)
            {
                var q = string.Empty;
                foreach (var i in model.GetType().GetProperties())
                {
                    q += $"{i.Name}={i.GetValue(model, null)?.ToString()}&";
                }
                if (!string.IsNullOrEmpty(q)) requestUri += $"?{q}";
            }
            if (!await accountService.CheckUserAuthAsync())
                return new PostRespModel { StatusCode = HttpStatusCode.Unauthorized };

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var userSession = await localStorage.GetItemAsync<UserSessionModel>("UserSession");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userSession.AccessToken);
            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<PostRespModel>(await response.Content.ReadAsStringAsync());
            if (showErrorMsg)
                toastService.ShowError($"AN {response.StatusCode} ERROR OCCURED", "ERROR");

            return new PostRespModel { StatusCode = response.StatusCode };
        }

        public async Task<PostRespModel> PostAsync(string requestUri, object model, bool showErrorMsg)
        {
              if (!await accountService.CheckUserAuthAsync())
                return new PostRespModel { StatusCode = HttpStatusCode.Unauthorized };
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            var userSession = await localStorage.GetItemAsync<UserSessionModel>("UserSession");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userSession.AccessToken);
            request.Content = new StringContent(JsonConvert.SerializeObject(model));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<PostRespModel>(await response.Content.ReadAsStringAsync());

            if (showErrorMsg)
                toastService.ShowError($"AN {response.StatusCode} ERROR OCCURED", "ERROR");

            return new PostRespModel { StatusCode = response.StatusCode };
        } 
    }
}
