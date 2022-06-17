using System.Collections.Generic;
using System.Linq;

namespace PropMng.Api.Models.Models
{
    public class UserRoleModel
    {
        public UserRoleModel()
        {
            Roles = new List<string>();
        }
        public bool IsValid(params string[] roles) 
            => Roles.Join(roles, a=>a.ToLower(),b=>b.ToLower(),(x,y)=>x).Any();

      public List<string> Roles { get; set; } 
    }
}
