using Microsoft.AspNetCore.Components;
using PropMng.Api.Models.HrArea.Inv;
using PropMng.Api.Models.Models;
using System.Threading.Tasks;

namespace PropMng.Web.Pages.HrArea.Inv
{
    public partial class InvItem
    {
        [CascadingParameter(Name = "UserRole")] public UserRoleModel UserRole { get; set; }
        [Parameter] public InvModel Item { get; set; }
        [Parameter] public int Index { get; set; }
        [Parameter] public EventCallback<long> OnEditItem { get; set; }
        [Parameter] public EventCallback<long> OnDeleteItem { get; set; }

        private async Task EditItem()
        {
            await OnEditItem.InvokeAsync(Item.Id);
        }
        private async Task DeleteItem()
        {
            await OnDeleteItem.InvokeAsync(Item.Id);
        }
    }
}
