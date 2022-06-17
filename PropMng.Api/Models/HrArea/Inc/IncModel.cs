using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PropMng.Api.Models.HrArea.Inc
{
    public class IncModel
    {
        public IncModel()
        {

        }
        public long Id { get; set; }
        public DateTime IncomeDate { get; set; }
        public decimal Amount { get; set; }
        public long InvoiceId { get; set; }
        public int TypeId { get; set; }
        public string ReceiptNum { get; set; }
        public long InvoiceNum { get; set; }
        public string PersonName { get; set; }
        public string PropName { get; set; }
        public string UnitName { get; set; }
        public DateTime CreatedDate { get; internal set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

        public List<string> RequiredFields()
        {
            var lst = new List<string>();

            if (Amount==0) lst.Add(nameof(Amount));
            return lst;
        }
    }
}
