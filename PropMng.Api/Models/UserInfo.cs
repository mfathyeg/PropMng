using System.Collections.Generic;

namespace PropMng.Api.Models
{
    public class UserInfo
    {
        public UserInfo()
        {
            Roles = new List<string>();
        }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public long PersonId { get; set; }
        public long LogId { get; set; }
        public List<string> Roles { get;   set; }
    }
}
