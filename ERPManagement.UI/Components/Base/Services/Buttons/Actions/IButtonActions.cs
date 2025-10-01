using ERPManagement.Application.Shared.Enums;
using ERPManagement.UI.GeneralClasses;
using Microsoft.Extensions.Localization;

namespace ERPManagement.UI.Components.Base.Services.Buttons.Actions
{
    public interface IButtonActions<TModel>
    {
        // --- State & Config used by buttons UI ---
        FormState State { get; }
        HiddenButtonsConfig HiddenButtons { get; }
        bool IsArabic { get; }
        string FormName { get; }
        IStringLocalizer Localizer { get; }

        // --- Actions ---
        Task NewEntity();
        Task EditEntity();
        Task DeleteEntity();
        Task<bool> SaveEntity();
        Task SaveAndCloseEntity();
        Task CancelEntity();


    }
}
