using ERPManagement.Application.Contracts.Infrastructure.Services;
using ERPManagement.Application.Shared.Enums;
using ERPManagement.UI.Components.Base.Services;
using ERPManagement.UI.Components.Base.Services.Buttons.Actions;
using ERPManagement.UI.GeneralClasses;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Data;
using System.Text.Json;

public class ButtonActionsAdapter<TModel> : IButtonActions<TModel>
    where TModel : new()
{
    private readonly IEntityFormActions<TModel> _form;
    private readonly IJSRuntime _js;
    private readonly IServiceProvider _serviceProvider;
    public ButtonActionsAdapter(IEntityFormActions<TModel> form, IJSRuntime js, IServiceProvider serviceProvider)
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
        ClearControls();
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
            if (_form.CheckBeforeDelete != null)
            {
                var checkMessage = _form.CheckBeforeDelete(_form.CurrentObject);
                if (!string.IsNullOrEmpty(checkMessage))
                {
                    await _js.InvokeVoidAsync("Swal.fire", new
                    {
                        icon = "error",
                        title = _form.IsArabic ? "خطأ" : "Error",
                        text = checkMessage,
                    });
                    return;
                }
            }
            var deleted = _form.OnDelete(_form.CurrentObject);
            if (deleted)
            {
                await ShowSuccessMessage("Delete", "تم حذف البيانات بنجاح", "Data deleted successfully");
                _form.Data?.Rows.Remove((DataRow)_form.SelectedRow);
                _form.CurrentObject = new TModel();
                _form.State = FormState.View;
                _form.Refresh();
            }
        }
    }

    public async Task<bool> SaveEntity()
    {
        if (_form.CurrentObject != null)
        {
            _form.ValidationErrors.Clear();
            if (ValidateModel(_form.CurrentObject))
            {
                if (State == FormState.Add && _form.OnInsert != null)
                {
                    var id = _form.OnInsert(_form.CurrentObject);
                    typeof(TModel).GetProperty(typeof(TModel).Name + "ID")
                                  ?.SetValue(_form.CurrentObject, id);

                    _form.Data?.AddFromObject(_form.CurrentObject);
                    await ShowSuccessMessage("Save", "تم حفظ البيانات بنجاح", "Data saved successfully");
                }
                else if (State == FormState.Edit && _form.OnUpdate != null)
                {
                    _form.OnUpdate(_form.CurrentObject);
                    ((DataRow)_form.SelectedRow)?.UpdateFromObject(_form.CurrentObject);
                    _form.Data?.AcceptChanges();
                    await ShowSuccessMessage("Update", "تم تعديل البيانات بنجاح", "Data updated successfully");
                }
                ClearControls();
                _form.Refresh();
                return true;
            }
            else { ShowErrorsMessage(_form.ValidationErrors); return false; }

        }
        return false;
    }

    public async Task SaveAndCloseEntity()
    {
        bool saved = await SaveEntity();
        if (saved)
        {
            _form.State = FormState.View;
            _form.IsEnabled = false;
            _form.Refresh();// await _form.SaveAndCloseChangesAsync();
        }
    }

    public async Task CancelEntity()
    {
        _form.ValidationErrors.Clear();
        var confirmed = await ConfirmAndCancelAsync();
        if (confirmed)
        {
            _form.State = FormState.View;
            _form.IsEnabled = false;
            if (_form.OldObject is not null)
            {
                if (_form.SelectedRow != null)
                {
                    if (_form.MapRowToModel != null)
                        _form.CurrentObject = _form.MapRowToModel(_form.SelectedRow);
                    _form.OldObject = (TModel)(_form.CurrentObject as ICloneable)?.Clone() ?? _form.CurrentObject;
                }
            }
            _form.Refresh();
        }
    }
    protected bool ValidateModel(TModel model)
    {

        _form.ValidationErrors.Clear();

        var validator = _serviceProvider.GetService(typeof(IValidator<TModel>)) as IValidator<TModel>;
        if (validator == null)
        {
            _form.ValidationErrors = new List<string> { $"No validator found for {typeof(TModel).Name}" };
            return false;
        }

        var errors = validator.Validate(model)
                              .Where(e => !string.IsNullOrEmpty(e))
                              .ToList();

        _form.ValidationErrors = errors;

        return !_form.ValidationErrors.Any();
    }
    #region Cancel Confirmation
    protected async Task<bool> ConfirmAndCancelAsync()
    {
        var result = await _js.InvokeAsync<JsonElement>("Swal.fire", new
        {
            title = IsArabic ? "هل أنت متأكد؟" : "Are you sure?",
            text = IsArabic ? "سيتم تجاهل التعديلات الحالية!" : "Your changes will be discarded!",
            icon = "warning",
            showCancelButton = true,
            confirmButtonText = IsArabic ? "نعم، إلغاء" : "Yes, cancel",
            cancelButtonText = IsArabic ? "رجوع" : "Back"
        });

        return result.GetProperty("isConfirmed").GetBoolean();
    }
    #endregion
    private async Task ShowSuccessMessage(string action, string messageAr, string messageEn)
    {
        await _js.InvokeVoidAsync("Swal.fire", new
        {
            icon = "success",
            title = IsArabic ? action + " البيانات" : action + " data",
            text = IsArabic ? messageAr : messageEn
        });
    }
    private async Task ShowErrorsMessage(List<string> errors)
    {
        if (errors != null && errors.Any())
        {
            foreach (var error in errors)
            {
                await _js.InvokeVoidAsync("Swal.fire", new
                {
                    toast = true,
                    position = "top-end",
                    showConfirmButton = false,
                    timer = 2000,
                    icon = "error",
                    title = error
                });
            }
        }
    }
    private void ClearControls()
    {
        _form.CurrentObject = new TModel();

        var branchProp = typeof(TModel).GetProperty("BranchID");
        if (branchProp != null && branchProp.CanWrite)
            branchProp.SetValue(_form.CurrentObject, _form.CurrentBranchID);

        var idProp = typeof(TModel).GetProperty(typeof(TModel).Name + "ID");
        if (idProp != null && idProp.CanWrite)
            idProp.SetValue(_form.CurrentObject, -1);
        _form.ValidationErrors.Clear();
    }
}
