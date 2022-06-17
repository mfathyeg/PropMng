using PropMng.Api.Models.Models;
using System.Threading.Tasks;

namespace PropMng.Web.Data
{
    public interface IAccountService
    {
        Task<bool> CheckUserAuthAsync();
        Task<LoginBaseDto> LoginAsync(LoginBaseModel model);
    }
}
