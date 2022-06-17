using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Inv;
using PropMng.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace PropMng.Web.Pages.HrArea.Inv
{
    public partial class InvEditor
    {
        [Parameter] public string Url { get; set; }
        [Parameter] public InvModel Item { get; set; }
        [Parameter] public EventCallback<bool> OnBack { get; set; }
        [Inject] public ISystemService SystemService { get; set; }
        [Inject] public IToastService ToastService { get; set; }

        private List<LookupModel> props = new List<LookupModel>();
        private List<LookupModel> units = new List<LookupModel>();
        private List<LookupModel> customers = new List<LookupModel>();

        protected override async Task OnInitializedAsync()
        {
            await LoadProps();
            await LoadCustomers();
        }

        private async Task PropChanged(ChangeEventArgs e)
        {
            units = new List<LookupModel>();
            if (e.Value!=null && long.TryParse(e.Value.ToString(), out long propId))
            {
                await LoadUnits(propId);
                Item.PropId =propId;
            }

        }

        private async Task UnitChanged(ChangeEventArgs e)
        {
            if (e.Value!=null && long.TryParse(e.Value.ToString(), out long unitId))
            {
                Item.UnitId= unitId;
                var rsp = await SystemService.GetAsync($"{Url}GetUnitInfo/?unitId={unitId}", null, true);
                if (rsp.IsSucceed)
                {

                    var o = rsp.GetDsrlzT<LookupModel>();
                    Item.Utilities = o.Details;
                    Item.MonthlyRent = Convert.ToDecimal(o.Name);
                }
            }

        }

        private async Task LoadProps()
        {
            var rsp = await SystemService.GetAsync($"{Url}GetProps/", Item, true);
            if (rsp.IsSucceed)
                props = rsp.GetDsrlzT<List<LookupModel>>();
        }
        private async Task LoadUnits(long propId)
        {
            var rsp = await SystemService.GetAsync($"{Url}GetUnits/?propId={propId}", null, true);
            if (rsp.IsSucceed)
                units = rsp.GetDsrlzT<List<LookupModel>>();
        }

        private async Task LoadCustomers()
        {
            var rsp = await SystemService.GetAsync($"{Url}GetCustomers/", Item, true);
            if (rsp.IsSucceed)
                customers = rsp.GetDsrlzT<List<LookupModel>>();
        }
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
                Item.RequiredFields().ForEach(a => ToastService.ShowError($"{a} IS REQUIRED", "REQUIRED FIELDS"));
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
