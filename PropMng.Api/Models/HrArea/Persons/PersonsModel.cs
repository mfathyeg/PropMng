using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PropMng.Api.Models.HrArea.Persons
{
    public class PersonsModel
    {
        public PersonsModel()
        {
        }
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string IdNum { get; set; }
        [Required]
        public string PhoneNum { get; set; }
        public bool IsCorp { get; set; }
        public bool IsMale { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string Details { get; set; }
        public string CrName { get; set; }
        public string CrNum { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

        public List<string> RequiredFields()
        {
            var lst = new List<string>();

            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
                lst.Add("Name");
            if (string.IsNullOrEmpty(IdNum)) lst.Add(nameof(IdNum));
            if (string.IsNullOrEmpty(PhoneNum)) lst.Add(nameof(PhoneNum));
            return lst;
        }
    }
}
