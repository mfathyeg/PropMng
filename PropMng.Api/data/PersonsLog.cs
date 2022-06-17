using System;
using System.Collections.Generic;

#nullable disable

namespace PropMng.Api.data
{
    public partial class PersonsLog
    {
        public long RecId { get; set; }
        public long LogId { get; set; }

        public virtual Log Log { get; set; }
        public virtual Person Rec { get; set; }
    }
}
