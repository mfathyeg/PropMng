using Blazored.LocalStorage;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PropMng.Web.Data
{
    public class ValidationHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService localStorageService;
        private readonly IAccountService accountService;

        public ValidationHeaderHandler(ILocalStorageService localStorageService, IAccountService accountService)
        {
            this.localStorageService = localStorageService;
            this.accountService = accountService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("Authorization"))
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
