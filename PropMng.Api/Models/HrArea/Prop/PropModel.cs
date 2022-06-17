using Newtonsoft.Json;
using PropMng.Api.Models.HrArea.PropUnit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PropMng.Api.Models.HrArea.Prop
{
    public class PropModel
    {
        public PropModel()
        {
           
        }
        public long Id { get; set; }
        public string Code { get; set; }
        [Required]
        public string CmName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string CrNum { get; set; }
        [Required]
        public string TaxRegNum { get; set; }
        
        public override string ToString() => JsonConvert.SerializeObject(this);

        public List<string> RequiredFields()
        {
            var lst = new List<string>();

            if (string.IsNullOrEmpty(CmName)) lst.Add(nameof(CmName));
            return lst;
        }
    }
}
