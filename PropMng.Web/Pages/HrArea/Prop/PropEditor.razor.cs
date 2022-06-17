using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using PropMng.Api.Models.HrArea.Prop;
using PropMng.Web.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace PropMng.Web.Pages.HrArea.Prop
{
    public partial class PropEditor
    {
        [Parameter] public string Url { get; set; }
        [Parameter] public PropModel Item { get; set; }
        [Parameter] public EventCallback<bool> OnBack { get; set; }
        [Inject] public ISystemService SystemService { get; set; }
        [Inject] public IToastService ToastService { get; set; }

        private async Task CancelItem()
        {
            await OnBack.InvokeAsync(false);
        }
        private async Task SaveItem()
        {
            if (PageValid())
            {
                var resp = Item.Id==0 ? await SystemService.PostAsync($"{Url}Create/", Item, true)
                    : await SystemService.PostAsync($"{Url}Update/", Item, true);
                if (resp.IsSucceed)
                {
                    ToastService.ShowSuccess("ITEM HAS BEEN SAVE SUCCESSFULLY", "DONE");
                    await OnBack.InvokeAsync(true);
                }
                else if (resp.Errors!=null &&  resp.Errors.Any())
                {
                    ShowErrors(resp.Errors);
                }
            }
        }
        private bool PageValid()
        {
            var valid = true;
            if (Item.RequiredFields().Any())
            {
                Item.RequiredFields().ForEach(a=>ToastService.ShowError($"{a} IS REQUIRED","REQUIRED FIELDS"));
                valid = false;
            }
            return valid;
        }
        private void ShowErrors(List<int> errors)
        {
            if (errors.Contains(1))
                ToastService.ShowError("ITEM IS NOT FOUND", "ERROR");
            if (errors.Contains(2))
                ToastService.ShowError("ITEM ALREADY EXISTS", "ERROR");
        } 
    }
}
