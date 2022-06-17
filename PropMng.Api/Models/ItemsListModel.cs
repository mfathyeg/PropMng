using System.Collections.Generic;

namespace PropMng.Api.Models.Models
{
    public class ItemsListModel<T>
    {
        public List<T> Items { get; set; }
        public int ItemsCount { get; set; }
    }
}
