using System;
using System.Collections.Generic;

#nullable disable

namespace PropMng.Api.data
{
    public partial class PropsLog
    {
        public long RecId { get; set; }
        public long LogId { get; set; }

        public virtual Log Log { get; set; }
        public virtual Prop Rec { get; set; }
    }
}
