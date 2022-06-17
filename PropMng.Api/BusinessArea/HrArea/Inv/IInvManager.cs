using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Inv;
using PropMng.Api.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropMng.Api.BusinessArea.HrArea.Inv
{
    public interface IInvManager
    {
        Task<PostResponseModel<InvModel>> Create(UserInfo userInfo, string screenCode, InvModel model);
        Task<PostResponseModel<bool>> Delete(UserInfo userInfo, string screenCode, long id);
        Task<PostResponseModel<ItemsListModel<InvModel>>> GetAll(UserInfo userInfo, string screenCode, InvFilterModel f);
        Task<PostResponseModel<InvModel>> GetById(UserInfo userInfo, string screenCode, long id);
        Task<PostResponseModel<List<LookupModel>>> GetCustomers( );
        Task<PostResponseModel<List<LookupModel>>> GetProps();
        Task<PostResponseModel<List<LookupModel>>> GetUnits(long propId);
        Task<PostResponseModel<bool>> Update(UserInfo userInfo, string screenCode, InvModel model);
        Task<PostResponseModel<LookupModel>> GetUnitInfo(long unitId);
    }
}