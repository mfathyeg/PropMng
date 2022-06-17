using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Blazored.LocalStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using PropMng.Api.Models.Models;
using PropMng.Api.Models;

namespace PropMng.Web.Data
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public AccountService(IConfiguration configuration, HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;

            httpClient.BaseAddress = new Uri(configuration["ServicesBaseAddresses:ServicesAddress"]);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "BlozerServer");
        }

        public async Task<LoginBaseDto> LoginAsync(LoginBaseModel model)
        {
            try
            {
                var response = await GetPostAsync("System/Account/Login", model);
                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<LoginBaseDto>(await response.Content.ReadAsStringAsync()) : null;
            }
            catch (Exception)
            {
                return null;
            }

        }
        

        public async Task<bool> CheckUserAuthAsync()
        {
            var userSession = await localStorage.GetItemAsync<UserSessionModel>("UserSession");
            if (userSession == null) return false;
            var response = await GetPostAsync("System/Account/CheckUserAuth", new RefreshTokenModel
            {
                AccessToken = userSession.AccessToken,
                RefreshToken = userSession.RefreshToken
            });
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var newTokens = JsonConvert.DeserializeObject<LoginBaseDto>(await response.Content.ReadAsStringAsync());
                if (newTokens.StatusId == (int)EnmLoginStatus.Success)
                {
                    if (newTokens.RefreshToken != userSession.RefreshToken)
                        await localStorage.SetItemAsync("UserSession", new UserSessionModel { UserName = userSession.UserName, RefreshToken = newTokens.RefreshToken, AccessToken = newTokens.AccessToken });
                    return true;
                }
            }
            //await localStorage.RemoveItemAsync("UserSession");
            return false;
        }
         
            
        private async Task<HttpResponseMessage> GetPostAsync(string urlString, object obj)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, urlString);
            var userSession = await localStorage.GetItemAsync<UserSessionModel>("UserSession");
            if (userSession != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userSession.AccessToken);
            request.Content = new StringContent(JsonConvert.SerializeObject(obj));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await httpClient.SendAsync(request);
        }
    }
}
