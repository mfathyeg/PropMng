using System;
using System.Collections.Generic;

#nullable disable

namespace PropMng.Api.data
{
    public partial class LogsEnterance
    {
        public LogsEnterance()
        {
            LogsTokens = new HashSet<LogsToken>();
        }

        public long Id { get; set; }
        public string UserId { get; set; }
        public string IpAddress { get; set; }

        public virtual Log IdNavigation { get; set; }
        public virtual AspNetUser User { get; set; }
        public virtual ICollection<LogsToken> LogsTokens { get; set; }
    }
}
