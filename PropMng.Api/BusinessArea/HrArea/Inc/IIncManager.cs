using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Inc;
using PropMng.Api.Models.HrArea.Persons;
using PropMng.Api.Models.HrArea.PropUnit;
using PropMng.Api.Models.Models;
using System.Threading.Tasks;

namespace PropMng.Api.BusinessArea.HrArea.Inc
{
    public interface IIncManager
    {
        Task<PostResponseModel<IncModel>> Create(UserInfo userInfo, string screenCode, IncModel model);
        Task<PostResponseModel<bool>> Delete(UserInfo userInfo, string screenCode, long id);
        Task<PostResponseModel<ItemsListModel<IncModel>>> GetAll(UserInfo userInfo, string screenCode, IncFilterModel f);
        Task<PostResponseModel<IncModel>> GetById(UserInfo userInfo, string screenCode, long id);
        Task<PostResponseModel<bool>> Update(UserInfo userInfo, string screenCode, IncModel model);
    }

}
