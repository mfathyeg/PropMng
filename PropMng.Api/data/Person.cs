using System;
using System.Collections.Generic;

#nullable disable

namespace PropMng.Api.data
{
    public partial class Person
    {
        public Person()
        {
            AspNetUsers = new HashSet<AspNetUser>();
            Invoices = new HashSet<Invoice>();
            PersonsLogs = new HashSet<PersonsLog>();
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string IdNum { get; set; }
        public string PhoneNum { get; set; }
        public bool IsCorp { get; set; }
        public bool IsMale { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsTrash { get; set; }
        public string Details { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual PersonsCorp PersonsCorp { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<PersonsLog> PersonsLogs { get; set; }
    }
}
