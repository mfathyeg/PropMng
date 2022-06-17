using System;
using System.Collections.Generic;

#nullable disable

namespace PropMng.Api.data
{
    public partial class Prop
    {
        public Prop()
        {
            PropsLogs = new HashSet<PropsLog>();
            PropsUnits = new HashSet<PropsUnit>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string CmName { get; set; }
        public string CrNum { get; set; }
        public string TaxRegNum { get; set; }
        public string Address { get; set; }
        public bool IsTrash { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<PropsLog> PropsLogs { get; set; }
        public virtual ICollection<PropsUnit> PropsUnits { get; set; }
    }
}
