using Microsoft.AspNetCore.Components;
using PropMng.Api.Models.HrArea.PropUnit;
using PropMng.Api.Models.Models;
using System.Threading.Tasks;

namespace PropMng.Web.Pages.HrArea.PropUnit
{
    public partial class PropUnitItem
    {
        [CascadingParameter(Name = "UserRole")] public UserRoleModel UserRole { get; set; }
        [Parameter] public PropUnitModel Item { get; set; }
        [Parameter] public int Index { get; set; }
        [Parameter] public EventCallback<long> OnEditItem { get; set; }
        [Parameter] public EventCallback<long> OnDeleteItem { get; set; }
        [Parameter] public EventCallback<long> OnViewInvoices { get; set; }

        private async Task EditItem()
        {
            await OnEditItem.InvokeAsync(Item.Id);
        }
        private async Task DeleteItem()
        {
            await OnDeleteItem.InvokeAsync(Item.Id);
        }

        private async Task ViewInvoices()
        {
            await OnViewInvoices.InvokeAsync(Item.Id);
        }
    }
}
