using System;
using System.Collections.Generic;

#nullable disable

namespace PropMng.Api.data
{
    public partial class LogsToken
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual LogsEnterance IdNavigation { get; set; }
    }
}
