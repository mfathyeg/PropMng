using Newtonsoft.Json;
using PropMng.Api.Models.SystemArea;

namespace PropMng.Api.Models.HrArea.Inc
{
    public class IncFilterModel : PagerFilterModel
    {
        public IncFilterModel()
        {
            PageSize = 15;
            PageNumber = 1;
        }
        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
