using ERPManagement.UI.Components.Base.Services.Buttons.Actions;
using ERPManagement.UI.Components.Base.Services.Buttons.Navigations;
using Microsoft.AspNetCore.Components;

namespace ERPManagement.UI.Components.Base
{
    public partial class DynamicButtons<TModel> : ComponentBase where TModel : new()
    {
        //[CascadingParameter] public PageLayoutComponent<TModel> BasePage { get; set; }

        //[CascadingParameter] public IButtonActions<TModel> BaseActions { get; set; }
        [Parameter] public IButtonActions<TModel>? BaseActions { get; set; }
        //[CascadingParameter] public IButtonNavigations<TModel>? BaseNavigations { get; set; }
        [Parameter] public IButtonNavigations<TModel>? BaseNavigations { get; set; }
        [Inject] protected NavigationManager navigationManager { get; set; }
        [Parameter] public string FormName { get; set; }
        [Parameter] public bool IsArabic { get; set; }
        private Task OnNewClick() => BaseActions?.NewEntity() ?? Task.CompletedTask;
        private Task OnEditClick() => BaseActions?.EditEntity() ?? Task.CompletedTask;
        private Task OnDeleteClick() => BaseActions?.DeleteEntity() ?? Task.CompletedTask;
        private Task OnSaveClick() => BaseActions?.SaveEntity() ?? Task.CompletedTask;
        private Task OnSaveAndCloseClick() => BaseActions?.SaveAndCloseEntity() ?? Task.CompletedTask;
        private Task OnCancelClick() => BaseActions?.CancelEntity() ?? Task.CompletedTask;
        private void Close()
        {
            navigationManager.NavigateTo("/index");
        }

    }

    // Non-generic partial class so IStringLocalizer<DynamicButtons> can bind to resources
    public partial class DynamicButtons { }
}
