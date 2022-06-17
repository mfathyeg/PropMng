using System;
using System.Collections.Generic;

#nullable disable

namespace PropMng.Api.data
{
    public partial class PersonsCorp
    {
        public long Id { get; set; }
        public string CrName { get; set; }
        public string CrNum { get; set; }

        public virtual Person IdNavigation { get; set; }
    }
}
