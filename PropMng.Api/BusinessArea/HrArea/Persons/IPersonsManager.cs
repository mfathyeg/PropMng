using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Persons;
using PropMng.Api.Models.HrArea.Persons;
using PropMng.Api.Models.Models;
using System.Threading.Tasks;

namespace PropMng.Api.BusinessArea.HrArea.Persons
{
    public interface IPersonsManager
    {
        Task<PostResponseModel<PersonsModel>> Create(UserInfo userInfo, string screenCode, PersonsModel model);
        Task<PostResponseModel<bool>> Delete(UserInfo userInfo, string screenCode, long id);
        Task<PostResponseModel<ItemsListModel<PersonsModel>>> GetAll(UserInfo userInfo, string screenCode, PersonsFilterModel f);
        Task<PostResponseModel<PersonsModel>> GetById(UserInfo userInfo, string screenCode, long id);
        Task<PostResponseModel<bool>> Update(UserInfo userInfo, string screenCode, PersonsModel model);
    }

}
