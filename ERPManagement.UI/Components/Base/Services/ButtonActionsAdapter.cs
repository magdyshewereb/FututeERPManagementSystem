using ERPManagement.Application.Shared.Enums;
using ERPManagement.UI.Components.Base.Services;
using ERPManagement.UI.GeneralClasses;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Text.Json;

namespace ERPManagement.UI.Components.Base
{
    public class ButtonActionsAdapter<TModel> : IButtonActions<TModel>
        where TModel : new()
    {
        private readonly PageLayoutComponent<TModel> _page;
        private readonly IJSRuntime _js;
        private readonly IServiceProvider _serviceProvider;

        public ButtonActionsAdapter(PageLayoutComponent<TModel> page, IJSRuntime js, IServiceProvider serviceProvider)
        {
            _page = page;
            _js = js;
            _serviceProvider = serviceProvider;
        }

        public FormState State => _page.State;
        public HiddenButtonsConfig HiddenButtons => _page.HiddenButtons;

        public bool IsArabic => _page.IsArabic;

        public string FormName => _page.FormName;

        public IStringLocalizer Localizer => _page.Localizer;

        public Task NewEntity()
        {
            _page.CurrentObject = new TModel();
            _page.IsEnabled = true;
            _page.State = FormState.Add;
            _page.SelectedRow = null;
            _page.Refresh();
            return Task.CompletedTask;
        }

        public Task EditEntity()
        {
            if (_page.SelectedRow == null) return Task.CompletedTask;
            _page.IsEnabled = true;
            _page.State = FormState.Edit;
            _page.Refresh();
            return _page.EditEntity;
        }

        public async Task DeleteEntity()
        {
            if (_page.CurrentObject == null || _page.OnDelete == null) return;

            var result = await _js.InvokeAsync<JsonElement>("Swal.fire", new
            {
                title = _page.IsArabic ? "هل أنت متأكد؟" : "Are you sure?",
                text = _page.IsArabic ? "لن تستطيع التراجع!" : "You won’t be able to revert this!",
                icon = "warning",
                showCancelButton = true,
                confirmButtonText = _page.IsArabic ? "نعم، احذف!" : "Yes, delete!",
                cancelButtonText = _page.IsArabic ? "إلغاء" : "Cancel"
            });

            bool isConfirmed = result.GetProperty("isConfirmed").GetBoolean();
            if (isConfirmed)
            {
                var deleted = _page.OnDelete(_page.CurrentObject);
                if (deleted)
                {
                    await _js.InvokeVoidAsync("Swal.fire", new
                    {
                        icon = "success",
                        title = _page.IsArabic ? "تم الحذف" : "Deleted",
                        text = _page.IsArabic ? "تم حذف البيانات بنجاح" : "Data deleted successfully",
                    });
                    _page.Data?.Rows.Remove(_page.SelectedRow);
                    _page.CurrentObject = new TModel();
                    _page.State = FormState.View;
                    _page.Refresh();

                }
            }
        }

        public Task SaveEntity()
        {
            if (_page.CurrentObject != null)
            {
                if (State == FormState.Add && _page.OnInsert != null)
                {
                    var id = _page.OnInsert(_page.CurrentObject);
                    typeof(TModel).GetProperty(typeof(TModel).Name + "ID")
                                  ?.SetValue(_page.CurrentObject, id);

                    _page.Data?.AddFromObject(_page.CurrentObject);
                }
                else if (State == FormState.Edit && _page.OnUpdate != null)
                {
                    _page.OnUpdate(_page.CurrentObject);
                    _page.SelectedRow?.UpdateFromObject(_page.CurrentObject);
                    _page.Data?.AcceptChanges();
                }

                _page.State = FormState.View;
                _page.Refresh();
            }
            return Task.CompletedTask;
        }

        public Task SaveAndCloseEntity()
        {
            SaveEntity();
            _page.State = FormState.View;
            _page.IsEnabled = false;

            return Task.CompletedTask;
        }

        public async Task CancelEntity()
        {
            var result = await _js.InvokeAsync<JsonElement>("Swal.fire", new
            {
                title = _page.IsArabic ? "هل أنت متأكد؟" : "Are you sure?",
                text = _page.IsArabic ? "سيتم تجاهل التعديلات الحالية!" : "Your changes will be discarded!",
                icon = "warning",
                showCancelButton = true,
                confirmButtonText = _page.IsArabic ? "نعم، إلغاء" : "Yes, cancel",
                cancelButtonText = _page.IsArabic ? "رجوع" : "Back"
            });

            if (result.GetProperty("isConfirmed").GetBoolean())
            {
                _page.State = FormState.View;
                _page.IsEnabled = false;
                _page.Refresh();
            }
        }
    }
}
