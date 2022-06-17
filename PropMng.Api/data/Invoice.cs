using System;
using System.Collections.Generic;

#nullable disable

namespace PropMng.Api.data
{
    public partial class Invoice
    {
        public Invoice()
        {
            Incomes = new HashSet<Income>();
            InvoicesLogs = new HashSet<InvoicesLog>();
        }

        public long Id { get; set; }
        public long PersonId { get; set; }
        public long UnitId { get; set; }
        public long Num { get; set; }
        public int StartYear { get; set; }
        public int StartMonth { get; set; }
        public int EndYear { get; set; }
        public int EndMonth { get; set; }
        public int Months { get; set; }
        public decimal MonthlyRent { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalAmountWithTax { get; set; }
        public string Utilities { get; set; }
        public string Details { get; set; }
        public bool IsTrash { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Person Person { get; set; }
        public virtual PropsUnit Unit { get; set; }
        public virtual ICollection<Income> Incomes { get; set; }
        public virtual ICollection<InvoicesLog> InvoicesLogs { get; set; }
    }
}
