using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropMng.Api.BusinessArea.HrArea.Persons;
using PropMng.Api.Infrastructure;
using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Persons;
using PropMng.Api.Models.Models;
using System.Threading.Tasks;

namespace PropMng.Api.Controllers.HrArea
{
    [ApiController]
    [Route("Hr/[controller]")]
    [CustomExceptionAttribute]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonsManager personsManager;

        public PersonsController(IPersonsManager personsManager)
        {
            this.personsManager=personsManager;
        }

        [HttpPost("Get")] 
        public async Task<PostResponseModel<ItemsListModel<PersonsModel>>> Get(PersonsFilterModel f)
            => await personsManager.GetAll(User.GetUserInfo(), ScreenCode, f);

        [HttpGet("GetById")]
        public async Task<PostResponseModel<PersonsModel>> GetById(long id)
           => await personsManager.GetById(User.GetUserInfo(), ScreenCode, id);

        [HttpPost("Create")]
        public async Task<PostResponseModel<PersonsModel>> Create(PersonsModel model)
          => await personsManager.Create(User.GetUserInfo(), ScreenCode, model);

        [HttpPost("Update")]
        public async Task<PostResponseModel<bool>> Update(PersonsModel model)
        => await personsManager.Update(User.GetUserInfo(), ScreenCode, model);

        [HttpPost("Delete/{id}")]
        public async Task<PostResponseModel<bool>> Delete(long id)
       => await personsManager.Delete(User.GetUserInfo(), ScreenCode, id);

        private string ScreenCode => ControllerContext.RouteData.Values["controller"].ToString();
    }
}