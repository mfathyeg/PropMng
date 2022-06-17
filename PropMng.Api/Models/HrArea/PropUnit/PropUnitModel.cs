using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PropMng.Api.Models.HrArea.PropUnit
{
    public class PropUnitModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Range(1,1000000)]
        public decimal MonthlyRent { get;   set; }
        public string Utilities { get;   set; }
        public string PropName { get; set; }

        [Required]
        [Range(1, 1000000)]
        public long PropId { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

        public List<string> RequiredFields()
        {
            var lst = new List<string>();

            if (PropId==0) lst.Add("Property Name");
            if (string.IsNullOrEmpty(Name)) lst.Add(nameof(Name));
            if (string.IsNullOrEmpty(Address)) lst.Add(nameof(Address));
            return lst;
        }
    }
}
