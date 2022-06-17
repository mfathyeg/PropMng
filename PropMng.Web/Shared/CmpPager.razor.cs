using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace PropMng.Web.Shared
{
    public partial class CmpPager
    {
        [Parameter]
        public int PageSize { get; set; }

        [Parameter]
        public int PageNumber { get; set; }

        [Parameter]
        public int ItemsCount { get; set; }

        [Parameter]
        public EventCallback<int> ChangePage { get; set; }


        private int StartIndex
        {
            get
            {
                return Math.Max(PageNumber - 5, 1);
            }
        }
        private int FinishIndex
        {
            get
            {
                return Math.Min(PageNumber + 5, TotalPages);
            }
        }

        private int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(ItemsCount / (decimal)PageSize);
            }
        }

        private async Task BtnPagerNUmberClick(int pageNumber)
        {
            await ChangePage.InvokeAsync(pageNumber);
        }
    }
}
