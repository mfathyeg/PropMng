using Newtonsoft.Json;
using PropMng.Api.Models.SystemArea;

namespace PropMng.Api.Models.HrArea.PropUnit
{
    public class PropUnitFilterModel : PagerFilterModel
    {
        public PropUnitFilterModel()
        {
            PageSize = 15;
            PageNumber = 1;
        }
        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
