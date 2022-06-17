using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Prop;
using PropMng.Api.Models.Models;
using System.Threading.Tasks;

namespace PropMng.Api.BusinessArea.HrArea.Prop
{
    public interface IPropManager
    {
        Task<PostResponseModel<PropModel>> Create(UserInfo userInfo, string screenCode, PropModel model);
        Task<PostResponseModel<bool>> Delete(UserInfo userInfo, string screenCode, long id);
        Task<PostResponseModel<ItemsListModel<PropModel>>> GetAll(UserInfo userInfo, string screenCode, PropFilterModel f);
        Task<PostResponseModel<PropModel>> GetById(UserInfo userInfo, string screenCode, long id);
        Task<PostResponseModel<bool>> Update(UserInfo userInfo, string screenCode, PropModel model);
    }

}
