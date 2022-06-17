using System;
using System.Collections.Generic;

#nullable disable

namespace PropMng.Api.data
{
    public partial class LogsError
    {
        public long Id { get; set; }
        public string IpAddress { get; set; }
        public string EnteredUserName { get; set; }
        public string EnteredPassword { get; set; }

        public virtual Log IdNavigation { get; set; }
    }
}
