using ERPManagement.Application.Shared.Enums;
using ERPManagement.UI.Components.Base.Services;
using ERPManagement.UI.GeneralClasses;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Data;
using System.Text.Json;

public class ButtonActionsAdapter<TModel> : IButtonActions<TModel>
    where TModel : new()
{
    private readonly IEntityForm<TModel> _form;
    private readonly IJSRuntime _js;
    private readonly IServiceProvider _serviceProvider;
    public ButtonActionsAdapter(IEntityForm<TModel> form, IJSRuntime js, IServiceProvider serviceProvider)
    {
        _form = form;
        _js = js;
        _serviceProvider = serviceProvider;
    }

    public FormState State => _form.State;
    public HiddenButtonsConfig HiddenButtons => _form.HiddenButtons;
    public bool IsArabic => _form.IsArabic;
    public string FormName => _form.FormName;
    public IStringLocalizer Localizer => _form.Localizer;

    public Task NewEntity()
    {
        _form.CurrentObject = new TModel();
        _form.IsEnabled = true;
        _form.State = FormState.Add;
        _form.SelectedRow = null;
        _form.Refresh();
        return Task.CompletedTask;
    }

    public Task EditEntity()
    {
        if (_form.SelectedRow == null) return Task.CompletedTask;
        _form.IsEnabled = true;
        _form.State = FormState.Edit;
        _form.Refresh();
        return Task.CompletedTask;
    }

    public async Task DeleteEntity()
    {
        if (_form.CurrentObject == null || _form.OnDelete == null) return;

        var result = await _js.InvokeAsync<JsonElement>("Swal.fire", new
        {
            title = _form.IsArabic ? "هل أنت متأكد؟" : "Are you sure?",
            text = _form.IsArabic ? "لن تستطيع التراجع!" : "You won’t be able to revert this!",
            icon = "warning",
            showCancelButton = true,
            confirmButtonText = _form.IsArabic ? "نعم، احذف!" : "Yes, delete!",
            cancelButtonText = _form.IsArabic ? "إلغاء" : "Cancel"
        });

        if (result.GetProperty("isConfirmed").GetBoolean())
        {
            var deleted = _form.OnDelete(_form.CurrentObject);
            if (deleted)
            {
                await _js.InvokeVoidAsync("Swal.fire", new
                {
                    icon = "success",
                    title = _form.IsArabic ? "تم الحذف" : "Deleted",
                    text = _form.IsArabic ? "تم حذف البيانات بنجاح" : "Data deleted successfully",
                });
                _form.Data?.Rows.Remove((DataRow)_form.SelectedRow);
                _form.CurrentObject = new TModel();
                _form.State = FormState.View;
                _form.Refresh();
            }
        }
    }

    public Task SaveEntity()
    {
        if (_form.CurrentObject != null)
        {
            if (State == FormState.Add && _form.OnInsert != null)
            {
                var id = _form.OnInsert(_form.CurrentObject);
                typeof(TModel).GetProperty(typeof(TModel).Name + "ID")
                              ?.SetValue(_form.CurrentObject, id);

                _form.Data?.AddFromObject(_form.CurrentObject);
            }
            else if (State == FormState.Edit && _form.OnUpdate != null)
            {
                _form.OnUpdate(_form.CurrentObject);
                ((DataRow)_form.SelectedRow)?.UpdateFromObject(_form.CurrentObject);
                _form.Data?.AcceptChanges();
            }

            _form.State = FormState.View;
            _form.Refresh();
        }
        return Task.CompletedTask;
    }

    public Task SaveAndCloseEntity()
    {
        SaveEntity();
        _form.State = FormState.View;
        _form.IsEnabled = false;
        return Task.CompletedTask;
    }

    public async Task CancelEntity()
    {
        var result = await _js.InvokeAsync<JsonElement>("Swal.fire", new
        {
            title = _form.IsArabic ? "هل أنت متأكد؟" : "Are you sure?",
            text = _form.IsArabic ? "سيتم تجاهل التعديلات الحالية!" : "Your changes will be discarded!",
            icon = "warning",
            showCancelButton = true,
            confirmButtonText = _form.IsArabic ? "نعم، إلغاء" : "Yes, cancel",
            cancelButtonText = _form.IsArabic ? "رجوع" : "Back"
        });

        if (result.GetProperty("isConfirmed").GetBoolean())
        {
            _form.State = FormState.View;
            _form.IsEnabled = false;
            _form.Refresh();
        }
    }
}
