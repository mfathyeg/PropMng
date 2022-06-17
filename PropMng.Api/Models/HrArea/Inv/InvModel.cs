using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PropMng.Api.Models.HrArea.Inv
{
    public class InvModel
    {
        public InvModel()
        {

        }
        public long Id { get; set; }
        public long PersonId { get; set; }

        [Required, Range(0, 1000, ErrorMessage = "*")]
        public long PropId { get; set; }

        [Required, Range(0, 1000, ErrorMessage = "*")]
        public long UnitId { get; set; }
        public long Num { get; set; }
        [Required, Range(2022, 2030, ErrorMessage = "*")]
        public int StartYear { get; set; }
        [Required, Range(1, 12, ErrorMessage = "*")]
        public int StartMonth { get; set; }

        [Required, Range(2022, 2030, ErrorMessage = "*")]
        public int EndYear { get; set; }
        [Required, Range(1, 12, ErrorMessage = "*")]
        public int EndMonth { get; set; }
        public int Months
            => (EndYear>=StartYear) && (EndMonth>=StartMonth||EndYear>StartYear)
            ? ((EndYear-StartYear)*12)+(EndMonth-StartMonth)+1 : 0;
        public decimal MonthlyRent { get; set; }
        public decimal TotalAmount => MonthlyRent*Months;
        public decimal Vat => Convert.ToDecimal(0.15);
        public decimal TotalAmountWithTax => TotalAmount + (TotalAmount*Vat);
        public string Utlities { get; set; }
        public string Details { get; set; }
        public string PersonName { get; set; }
        public string UnitName { get; set; }
        public string PropName { get; set; }
        public string Utilities { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

        public List<string> RequiredFields()
        {
            var lst = new List<string>();

            if (PersonId==0) lst.Add("Person");
            if (UnitId==0) lst.Add("Unit");
            return lst;
        }
    }
}
