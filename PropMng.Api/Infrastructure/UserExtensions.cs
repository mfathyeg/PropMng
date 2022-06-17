using PropMng.Api.Models;
using System;
using System.Linq;
using System.Security.Claims;

namespace PropMng.Api.Infrastructure
{
    public static class UserExtensions
    {
        public static UserInfo GetUserInfo(this ClaimsPrincipal claims)
        {
            if (claims == null) return null;
            var user = new UserInfo();
            user.UserName = claims.Identity.Name;
            user.LogId = Convert.ToInt64(claims.Claims.Where(a => a.Type == "logId").Select(a => a.Value).SingleOrDefault());
            user.PersonId = Convert.ToInt32(claims.Claims.Where(a => a.Type == "personId").Select(a => a.Value).SingleOrDefault());
            user.UserId = claims.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).SingleOrDefault();
            user.Roles=claims.Claims.Where(a => a.Type==ClaimTypes.Role).Select(a => a.Value).ToList();
            return user;
        }
    }
}
