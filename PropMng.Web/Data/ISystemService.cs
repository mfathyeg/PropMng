using PropMng.Web.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace PropMng.Web.Data
{
    public interface ISystemService
    {
        Task<PostRespModel> GetAsync(string requestUri, object model, bool showErrorMsg); 
        Task<PostRespModel> PostAsync(string requestUri, object model, bool showErrorMsg);
        Task<PostRespModel> GetAsync(string requestUri) => GetAsync(requestUri, null, false);
        Task<PostRespModel> GetAsync(string requestUri, bool showErrorMsg) => GetAsync(requestUri, null, showErrorMsg);
    }
}


//
