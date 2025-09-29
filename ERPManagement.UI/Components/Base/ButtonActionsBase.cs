namespace ERPManagement.UI.Components.Base
{
    public class ButtonActionsBase<TModel> //: IButtonActions<TModel> where TModel : new()
    {
        //public FormState State { get; set; } = FormState.View;
        //[Inject] protected IJSRuntime JS { get; set; }
        //public HiddenButtonsConfig HiddenButtons { get; set; } = new();
        //public TModel CurrentObject { get; set; }
        //protected TModel OldObject { get; set; }
        //#region CRUD Actions
        //#region Add
        //protected virtual async Task<bool> AddEntity()
        //{
        //    try
        //    {
        //        if (ValidateModel(CurrentObject))
        //        {
        //            if (OnInsert != null)
        //            {
        //                var id = OnInsert(CurrentObject);
        //                // لو الـ model فيه خاصية ID 
        //                typeof(TModel).GetProperty(typeof(TModel).Name + "ID")?.SetValue(CurrentObject, id);
        //            }

        //            await ShowSuccessMessage("Save", "تم حفظ البيانات بنجاح", "Data saved successfully");
        //            Data?.AddFromObject(CurrentObject);
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        //#endregion

        //#region Update
        //protected virtual async Task<bool> UpdateEntity()
        //{
        //    try
        //    {
        //        if (ValidateModel(CurrentObject))
        //        {
        //            if (OnUpdate != null) OnUpdate(CurrentObject);
        //            await ShowSuccessMessage("Update", "تم تعديل البيانات بنجاح", "Data updated successfully");

        //            if (SelectedRow != null)
        //                SelectedRow.UpdateFromObject(CurrentObject);

        //            Data?.AcceptChanges();
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        //#endregion

        //#region Delete
        //Task IButtonActions<TModel>.DeleteEntity()
        //{
        //    try
        //    {
        //        if (OnDelete != null)
        //        {
        //            var deleted = OnDelete(CurrentObject);
        //            if (deleted)
        //            {
        //                Data?.Rows.Remove(SelectedRow);
        //                await ShowSuccessMessage("Delete", "تم حذف البيانات بنجاح", "Data deleted successfully");
        //                return true;
        //            }
        //        }
        //        return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        //#endregion

        //public virtual Task NewEntity()
        //{
        //    ClearControls();
        //    IsEnabled = true;
        //    State = FormState.Add;
        //    SelectedRow = null;
        //    InvokeAsync(StateHasChanged);
        //    return OnNewEntity.HasDelegate ? OnNewEntity.InvokeAsync() : Task.CompletedTask;
        //}
        //public virtual Task EditEntity()
        //{
        //    if (SelectedRow == null) return Task.CompletedTask;
        //    IsEnabled = true;
        //    State = FormState.Edit;
        //    InvokeAsync(StateHasChanged);
        //    return OnEditEntity.HasDelegate ? OnEditEntity.InvokeAsync() : Task.CompletedTask;
        //}
        ////public virtual Task DeleteEntity()
        ////{
        ////    if (SelectedRow == null) return Task.CompletedTask;
        ////    State = FormState.View;
        ////    InvokeAsync(StateHasChanged);
        ////    return OnDeleteEntity.HasDelegate ? OnDeleteEntity.InvokeAsync() : Task.CompletedTask;
        ////}
        //public virtual Task SaveEntity()
        //{
        //    State = FormState.Add;
        //    InvokeAsync(StateHasChanged);
        //    return OnSaveEntity.HasDelegate ? OnSaveEntity.InvokeAsync() : Task.CompletedTask;
        //}
        //public virtual Task SaveAndCloseEntity()
        //{
        //    State = FormState.View;
        //    IsEnabled = false;
        //    ClearControls();
        //    InvokeAsync(StateHasChanged);
        //    return OnSaveAndCloseEntity.HasDelegate ? OnSaveAndCloseEntity.InvokeAsync() : Task.CompletedTask;
        //}
        //Task IButtonActions<TModel>.CancelEntity()
        //{
        //    //var confirmed = await ConfirmAndCancelAsync();
        //    //if (!confirmed) return false;

        //    //State = FormState.View;

        //    //if (OldObject is not null)
        //    //{
        //    //    if (SelectedRow != null)
        //    //    {
        //    //        if (MapRowToModel != null)
        //    //            CurrentObject = MapRowToModel(SelectedRow);
        //    //        OldObject = (TModel)(CurrentObject as ICloneable)?.Clone() ?? CurrentObject;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    ClearControls();
        //    //    SelectedRow = null;
        //    //}
        //    //ValidationErrors.Clear();
        //    //IsEnabled = false;
        //    //await InvokeAsync(StateHasChanged); 

        //}
        //#endregion



        //#region Cancel Confirmation
        //protected async Task<bool> ConfirmAndCancelAsync()
        //{
        //    var result = await JS.InvokeAsync<JsonElement>("Swal.fire", new
        //    {
        //        title = IsArabic ? "هل أنت متأكد؟" : "Are you sure?",
        //        text = IsArabic ? "سيتم تجاهل التعديلات الحالية!" : "Your changes will be discarded!",
        //        icon = "warning",
        //        showCancelButton = true,
        //        confirmButtonText = IsArabic ? "نعم، إلغاء" : "Yes, cancel",
        //        cancelButtonText = IsArabic ? "رجوع" : "Back"
        //    });

        //    return result.GetProperty("isConfirmed").GetBoolean();
        //}


        //#endregion
    }
}
