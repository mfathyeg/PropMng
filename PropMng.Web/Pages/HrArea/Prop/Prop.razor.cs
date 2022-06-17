using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PropMng.Api.Models.HrArea.Prop;
using PropMng.Api.Models.Models;
using PropMng.Web.Data;
using PropMng.Web.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace PropMng.Web.Pages.HrArea.Prop
{
    public partial class Prop
    {
        [CascadingParameter(Name = "UserRole")] public UserRoleModel UserRole { get; set; }
        [Inject] public IJSRuntime JsRuntime { get; set; }

        private EnmPageDisplay PageDisplay { get; set; } = EnmPageDisplay.Main;
        private ItemsListModel<PropModel> ItemsList { get; set; } = new ItemsListModel<PropModel>();
        private PropModel Item { get; set; }

        [CascadingParameter] public IModalService ModalService { get; set; }

        [Inject] IToastService ToastService { get; set; }

        [Inject] public ISystemService SystemService { get; set; }

        private bool IsLoading { get; set; }
        private PropFilterModel f = new PropFilterModel();

        private string url = "Hr/Prop/";
        private async Task PagerChangePage(int pagenumber)
        {
            f.PageNumber = pagenumber;
            await LoadItems();
        }

        private void BtnNewClick()
        {
            Item = new PropModel();
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
                ItemsList = resp.GetDsrlzT<ItemsListModel<PropModel>>();
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
            var resp = await SystemService.GetAsync($"{url}GetById?id={id}");
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                ToastService.ShowError("حدث خطأ أثناء جلب البيانات.", "خطأ");
            }
            else if (resp.IsSucceed)
            {
                Item = resp.GetDsrlzT<PropModel>();
                PageDisplay = EnmPageDisplay.Editor;
            }
            else
            {
                ShowErrors(resp.Errors);
            }
        }

        private void ShowErrors(List<int> errors)
        {
            if (errors.Contains(1))
                ToastService.ShowError("البيان غير متاح ", "خطأ");
            if (errors.Contains(2))
                ToastService.ShowError("البيان موجود مسبقًا", "خطأ");
        }
    }
}
