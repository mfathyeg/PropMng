using Microsoft.AspNetCore.Mvc;
using PropMng.Api.BusinessArea.HrArea.Prop;
using PropMng.Api.Infrastructure;
using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Prop;
using PropMng.Api.Models.Models;
using System.Threading.Tasks;

namespace PropMng.Api.Controllers.HrArea
{
    [ApiController]
    [Route("Hr/[controller]")]
    [CustomExceptionAttribute]
    public class PropController : ControllerBase
    {
        private readonly IPropManager propManager  ;

        public PropController(IPropManager propManager)
        {
            this.propManager=propManager;
        }

        [HttpPost("Get")]
        public async Task<PostResponseModel<ItemsListModel<PropModel>>> Get(PropFilterModel f)
            => await propManager.GetAll(User.GetUserInfo(), ScreenCode, f);

        [HttpGet("GetById")]
        public async Task<PostResponseModel<PropModel>> GetById(long id)
           => await propManager.GetById(User.GetUserInfo(), ScreenCode, id);

        [HttpPost("Create")]
        public async Task<PostResponseModel<PropModel>> Create(PropModel model)
          => await propManager.Create(User.GetUserInfo(), ScreenCode, model);

        [HttpPost("Update")]
        public async Task<PostResponseModel<bool>> Update(PropModel model)
        => await propManager.Update(User.GetUserInfo(), ScreenCode, model);

        [HttpPost("Delete/{id}")]
        public async Task<PostResponseModel<bool>> Delete(long id)
       => await propManager.Delete(User.GetUserInfo(), ScreenCode, id);

        private string ScreenCode => ControllerContext.RouteData.Values["controller"].ToString();
    }
}