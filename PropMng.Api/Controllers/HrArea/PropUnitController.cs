using Microsoft.AspNetCore.Mvc;
using PropMng.Api.BusinessArea.HrArea.PropUnit;
using PropMng.Api.Infrastructure;
using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.PropUnit;
using PropMng.Api.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropMng.Api.Controllers.HrArea
{
    [ApiController]
    [Route("Hr/[controller]")]
    [CustomExceptionAttribute]
    public class PropUnitController : ControllerBase
    {
        private readonly IPropUnitManager propUnitManager;

        public PropUnitController(IPropUnitManager propUnitManager)
        {
            this.propUnitManager=propUnitManager;
        }

        [HttpPost("Get")]
        public async Task<PostResponseModel<ItemsListModel<PropUnitModel>>> Get(PropUnitFilterModel f)
            => await propUnitManager.GetAll(User.GetUserInfo(), ScreenCode, f);

        [HttpGet("GetById")]
        public async Task<PostResponseModel<PropUnitModel>> GetById(long id)
           => await propUnitManager.GetById(User.GetUserInfo(), ScreenCode, id);

        [HttpPost("Create")]
        public async Task<PostResponseModel<PropUnitModel>> Create(PropUnitModel model)
          => await propUnitManager.Create(User.GetUserInfo(), ScreenCode, model);

        [HttpPost("Update")]
        public async Task<PostResponseModel<bool>> Update(PropUnitModel model)
        => await propUnitManager.Update(User.GetUserInfo(), ScreenCode, model);

        [HttpPost("Delete/{id}")]
        public async Task<PostResponseModel<bool>> Delete(long id)
            => await propUnitManager.Delete(User.GetUserInfo(), ScreenCode, id);

        [HttpGet("GetProps")]
        public async Task<PostResponseModel<List<LookupModel>>> GetProps()
           => await propUnitManager.GetProps();
        private string ScreenCode => ControllerContext.RouteData.Values["controller"].ToString();
    }
}