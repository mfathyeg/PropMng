using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Persons;
using PropMng.Api.Models.HrArea.PropUnit;
using PropMng.Api.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropMng.Api.BusinessArea.HrArea.PropUnit
{
    public interface IPropUnitManager
    {
        Task<PostResponseModel<PropUnitModel>> Create(UserInfo userInfo, string screenCode, PropUnitModel model);
        Task<PostResponseModel<bool>> Delete(UserInfo userInfo, string screenCode, long id);
        Task<PostResponseModel<ItemsListModel<PropUnitModel>>> GetAll(UserInfo userInfo, string screenCode, PropUnitFilterModel f);
        Task<PostResponseModel<PropUnitModel>> GetById(UserInfo userInfo, string screenCode, long id);
        Task<PostResponseModel<List<LookupModel>>> GetProps();
        Task<PostResponseModel<bool>> Update(UserInfo userInfo, string screenCode, PropUnitModel model);
    }

}
