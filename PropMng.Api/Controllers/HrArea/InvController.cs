using Microsoft.AspNetCore.Mvc;
using PropMng.Api.BusinessArea.HrArea.Inv;
using PropMng.Api.Infrastructure;
using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Inv;
using PropMng.Api.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropMng.Api.Controllers.HrArea
{
    [ApiController]
    [Route("Hr/[controller]")]
    [CustomExceptionAttribute]
    public class InvController : ControllerBase
    {
        private readonly IInvManager invManager;

        public InvController(IInvManager invManager)
        {
            this.invManager=invManager;
        }

        [HttpPost("Get")]
        public async Task<PostResponseModel<ItemsListModel<InvModel>>> Get(InvFilterModel f)
            => await invManager.GetAll(User.GetUserInfo(), ScreenCode, f);

        [HttpGet("GetById")]
        public async Task<PostResponseModel<InvModel>> GetById(long id)
           => await invManager.GetById(User.GetUserInfo(), ScreenCode, id);

        [HttpPost("Create")]
        public async Task<PostResponseModel<InvModel>> Create(InvModel model)
          => await invManager.Create(User.GetUserInfo(), ScreenCode, model);

        [HttpPost("Update")]
        public async Task<PostResponseModel<bool>> Update(InvModel model)
        => await invManager.Update(User.GetUserInfo(), ScreenCode, model);

        [HttpPost("Delete/{id}")]
        public async Task<PostResponseModel<bool>> Delete(long id)
       => await invManager.Delete(User.GetUserInfo(), ScreenCode, id);

        [HttpGet("GetCustomers")]
        public async Task<PostResponseModel<List<LookupModel>>> GetCustomers()
          => await invManager.GetCustomers();

        [HttpGet("GetProps")]
        public async Task<PostResponseModel<List<LookupModel>>> GetProps()
         => await invManager.GetProps();

        [HttpGet("GetUnits")]
        public async Task<PostResponseModel<List<LookupModel>>> GetUnits(long propId)
        => await invManager.GetUnits(propId);
         
        [HttpGet("GetUnitInfo")]
        public async Task<PostResponseModel<LookupModel>> GetUnitInfo(long unitId)
       => await invManager.GetUnitInfo(unitId);
        private string ScreenCode => ControllerContext.RouteData.Values["controller"].ToString();
    }
}