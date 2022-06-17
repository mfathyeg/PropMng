using Newtonsoft.Json;
using PropMng.Api.Models.SystemArea;

namespace PropMng.Api.Models.HrArea.Persons
{
    public class PersonsFilterModel : PagerFilterModel
    {
        public PersonsFilterModel()
        {
            PageSize = 15;
            PageNumber = 1;
        }
        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
