using Newtonsoft.Json;
using PropMng.Api.Models.SystemArea;

namespace PropMng.Api.Models.HrArea.Inv
{
    public class InvFilterModel : PagerFilterModel
    {
        public InvFilterModel()
        {
            PageSize = 15;
            PageNumber = 1;
        }
        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
