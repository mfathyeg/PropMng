using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropMng.Api.Models.SystemArea
{
    public abstract class PagerFilterModel
    {
        public virtual int PageNumber { get; set; } = 1;
        public virtual int PageSize { get; set; } = 10;
        public virtual string SearchWord { get; set; }
        public virtual string DcdSearchWord => HttpUtility.UrlDecode(SearchWord);
        public bool HasSearchWord => !string.IsNullOrEmpty(DcdSearchWord);
        public virtual List<string> SearchWordsList => DcdSearchWord?.Split("&")?.ToList()??new List<string>();
    }
}
