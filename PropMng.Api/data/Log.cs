using System;
using System.Collections.Generic;

#nullable disable

namespace PropMng.Api.data
{
    public partial class Log
    {
        public Log()
        {
            IncomesLogs = new HashSet<IncomesLog>();
            InverseEnterance = new HashSet<Log>();
            InvoicesLogs = new HashSet<InvoicesLog>();
            PersonsLogs = new HashSet<PersonsLog>();
            PropsLogs = new HashSet<PropsLog>();
            PropsUnitsLogs = new HashSet<PropsUnitsLog>();
        }

        public long Id { get; set; }
        public int OperationId { get; set; }
        public string ScreenCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? EnteranceId { get; set; }

        public virtual Log Enterance { get; set; }
        public virtual LogsEnterance LogsEnterance { get; set; }
        public virtual LogsError LogsError { get; set; }
        public virtual LogsOperation LogsOperation { get; set; }
        public virtual ICollection<IncomesLog> IncomesLogs { get; set; }
        public virtual ICollection<Log> InverseEnterance { get; set; }
        public virtual ICollection<InvoicesLog> InvoicesLogs { get; set; }
        public virtual ICollection<PersonsLog> PersonsLogs { get; set; }
        public virtual ICollection<PropsLog> PropsLogs { get; set; }
        public virtual ICollection<PropsUnitsLog> PropsUnitsLogs { get; set; }
    }
}
