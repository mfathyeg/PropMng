using Microsoft.AspNetCore.Mvc;
using PropMng.Api.BusinessArea.HrArea.Inc;
using PropMng.Api.Infrastructure;
using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Inc;
using PropMng.Api.Models.Models;
using System.Threading.Tasks;

namespace PropMng.Api.Controllers.HrArea
{
    [ApiController]
    [Route("Hr/[controller]")]
    [CustomExceptionAttribute]
    public class IncController : ControllerBase
    {
        private readonly IIncManager incManager;

        public IncController(IIncManager incManager)
        {
            this.incManager=incManager;
        }

        [HttpPost("Get")]
        public async Task<PostResponseModel<ItemsListModel<IncModel>>> Get(IncFilterModel f)
            => await incManager.GetAll(User.GetUserInfo(), ScreenCode, f);

        [HttpGet("GetById")]
        public async Task<PostResponseModel<IncModel>> GetById(long id)
           => await incManager.GetById(User.GetUserInfo(), ScreenCode, id);

        [HttpPost("Create")]
        public async Task<PostResponseModel<IncModel>> Create(IncModel model)
          => await incManager.Create(User.GetUserInfo(), ScreenCode, model);

        [HttpPost("Update")]
        public async Task<PostResponseModel<bool>> Update(IncModel model)
        => await incManager.Update(User.GetUserInfo(), ScreenCode, model);

        [HttpPost("Delete/{id}")]
        public async Task<PostResponseModel<bool>> Delete(long id)
       => await incManager.Delete(User.GetUserInfo(), ScreenCode, id);

        private string ScreenCode => ControllerContext.RouteData.Values["controller"].ToString();
    }
}