using PropMng.Api.DataArea.Identity;
using PropMng.Api.Models;
using PropMng.Api.Models.Models;
using System.Threading.Tasks;

namespace PropMng.Api.BusinessArea.SystemArea
{
    public interface IAccountManager
    {
        Task<ChangePasswordDto> ChangePassword(ChangeMyPasswordModel model, string userId);
        Task<PostResponseModel<RoleModel>> CreateRole(RoleModel model);
        Task<PostResponseModel<UserRoleModel>> GetRoles(string userName);
        Task<PostResponseModel<LinkRoleModel>> LinkRole(string userName, LinkRoleModel model);
        Task<LoginBaseDto> Login(LoginBaseModel model);
        Task<LoginBaseDto> RefreshToken(RefreshTokenModel model);
    }

}
