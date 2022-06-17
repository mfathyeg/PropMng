using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PropMng.Api.Models.HrArea.PropUnit;
using PropMng.Api.Models.Models;
using PropMng.Web.Data;
using PropMng.Web.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace PropMng.Web.Pages.HrArea.PropUnit
{
    public partial class PropUnit
    {
        [CascadingParameter(Name = "UserRole")] public UserRoleModel UserRole { get; set; }
        [Inject] public IJSRuntime JsRuntime { get; set; }

        private EnmPageDisplay PageDisplay { get; set; } = EnmPageDisplay.Main;
        private ItemsListModel<PropUnitModel> ItemsList { get; set; } = new ItemsListModel<PropUnitModel>();
        private PropUnitModel Item { get; set; }

        [CascadingParameter] public IModalService ModalService { get; set; }

        [Inject] IToastService ToastService { get; set; }

        [Inject] public ISystemService SystemService { get; set; }

        private bool IsLoading { get; set; }
        private PropUnitFilterModel f = new PropUnitFilterModel();

        private string url = "Hr/PropUnit/";
        private async Task PagerChangePage(int pagenumber)
        {
            f.PageNumber = pagenumber;
            await LoadItems();
        }

        private void BtnNewClick()
        {
            Item = new PropUnitModel();
            PageDisplay = EnmPageDisplay.Editor;
        }
        private async Task ClearSearchWord()
        {
            f.SearchWord = string.Empty;
            await LoadItems();
        }

        private async Task InputChanged(ChangeEventArgs e)
        {
            if (e.Value != null)
            {
                f.SearchWord = HttpUtility.UrlEncode(e.Value.ToString());
                await LoadItems();
            }
        }

        private async Task Back(bool e)
        {
            if (e)
                await LoadItems();
            PageDisplay = EnmPageDisplay.Main;
        }


        private async Task LoadItems()
        {
            var resp = await SystemService.PostAsync($"{url}Get", f, true);
            if (resp.IsSucceed)
            {
                ItemsList = resp.GetDsrlzT<ItemsListModel<PropUnitModel>>();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            await LoadItems();
            IsLoading = false;
        }

        private async Task BtnRefreshClick()
        {
            IsLoading = true;
            await LoadItems();
            IsLoading = false;
        }

        private async Task DeleteItem(long id)
        {
            if (!await JsRuntime.InvokeAsync<bool>("confirm", $"ARE YOU SURE?"))
                return;
            var resp = await SystemService.PostAsync($"{url}Delete/{id}", null, true);
            if (resp.IsSucceed)
            {
                await LoadItems();
                ToastService.ShowSuccess("ITEM HAS BEEN DELETED", "DONE");
            }
            else if (resp.Errors!=null)
            {
                ShowErrors(resp.Errors);
            }
        }

        private async Task EditItem(long id)
        {
            var resp = await SystemService.GetAsync($"{url}GetById?id={id}", true);
            if (resp.IsSucceed)
            {
                Item = resp.GetDsrlzT<PropUnitModel>();
                PageDisplay = EnmPageDisplay.Editor;
            }
            else if (resp.Errors!=null)
            {
                ShowErrors(resp.Errors);
            }
        }
        private async Task ViewInvoices(long id)
        {
            var resp = await SystemService.GetAsync($"{url}GetById?id={id}", true);
            if (resp.IsSucceed)
            {
                Item = resp.GetDsrlzT<PropUnitModel>();
                PageDisplay = EnmPageDisplay.Editor;
            }
            else if (resp.Errors!=null)
            {
                ShowErrors(resp.Errors);
            }
        }

        private void ShowErrors(List<int> errors)
        {
            if (errors.Contains(1))
                ToastService.ShowError("ITEM IS NOT EXISTS", "ERROR");
            if (errors.Contains(2))
                ToastService.ShowError("ITEM IS ALREADY EXISTED", "ERROR");
            if (errors.Contains(5))
                ToastService.ShowError("ITEM IS RELATED", "ERROR");
        }
    }
}
