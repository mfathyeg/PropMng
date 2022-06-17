using Gdnc.Services.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PropMng.Api.BusinessArea.SystemArea;
using PropMng.Api.DataArea.Identity;
using PropMng.Api.Models;
using PropMng.Api.Models.Models;
using System.Threading.Tasks;

namespace PropMng.Api.Controllers.SystemArea
{
    [ApiController]
    [Route("System/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IAccountManager accountManager;

        public AccountController(IAccountManager accountManager
            , UserManager<CustomUser> userManager
            , IUnitOfWork unitOfWork)
        {
            this.accountManager = accountManager;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("CheckUserAuth")]
        [AllowAnonymous]
        public async Task<LoginBaseDto> CheckUserAuth(RefreshTokenModel model)
        {
            if (User.Identity.IsAuthenticated)
                return new LoginBaseDto { StatusId = (int)EnmLoginStatus.Success, AccessToken = model.AccessToken, RefreshToken = model.RefreshToken };

            return await accountManager.RefreshToken(model);
        }


        [HttpPost("IsAuthonticated")]
        public PostResponseModel<bool> IsAuthonticated()
        {
            return new PostResponseModel<bool>
            {
                IsSucceed = true,
                GetT = true
            };
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<LoginBaseDto> Login(LoginBaseModel model)
        {
            return await accountManager.Login(model);
        }

        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<LoginBaseDto> RefreshToken(RefreshTokenModel model)
        {
            return await accountManager.RefreshToken(model);
        }

        [HttpPost("CreateRole")]
        public async Task<PostResponseModel<RoleModel>> CreateRole(RoleModel model) => await accountManager.CreateRole(model);

        [HttpPost("LinkRole")]
        public async Task<PostResponseModel<LinkRoleModel>> LinkRole(LinkRoleModel model) => await accountManager.LinkRole(User.Identity.Name, model);

        [HttpGet("UserRole")]
        public async Task<PostResponseModel<UserRoleModel>> GetRoles() => await accountManager.GetRoles(User.Identity.Name);
    }
}