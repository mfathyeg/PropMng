using Newtonsoft.Json;
using PropMng.Api.Models.SystemArea;

namespace PropMng.Api.Models.HrArea.Prop
{
    public class PropFilterModel : PagerFilterModel
    {
        public PropFilterModel()
        {
            PageSize = 15;
            PageNumber = 1;
        }
        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
