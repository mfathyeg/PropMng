using System;
using System.Collections.Generic;

#nullable disable

namespace PropMng.Api.data
{
    public partial class LogsOperation
    {
        public long Id { get; set; }
        public string Details { get; set; }

        public virtual Log IdNavigation { get; set; }
    }
}
