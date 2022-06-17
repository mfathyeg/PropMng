using System;
using System.Collections.Generic;

#nullable disable

namespace PropMng.Api.data
{
    public partial class PropsUnit
    {
        public PropsUnit()
        {
            Invoices = new HashSet<Invoice>();
            PropsUnitsLogs = new HashSet<PropsUnitsLog>();
        }

        public long Id { get; set; }
        public long Num { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal MonthlyRent { get; set; }
        public string Address { get; set; }
        public string Utilities { get; set; }
        public long PropId { get; set; }
        public bool IsTrash { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Prop Prop { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<PropsUnitsLog> PropsUnitsLogs { get; set; }
    }
}
