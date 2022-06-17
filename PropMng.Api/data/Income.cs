using System;
using System.Collections.Generic;

#nullable disable

namespace PropMng.Api.data
{
    public partial class Income
    {
        public Income()
        {
            IncomesLogs = new HashSet<IncomesLog>();
        }

        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public int TypeId { get; set; }
        public string ReceiptNum { get; set; }
        public bool IsTrash { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual ICollection<IncomesLog> IncomesLogs { get; set; }
    }
}
