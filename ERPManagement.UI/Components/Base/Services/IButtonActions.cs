using ERPManagement.Application.Shared.Enums;
using ERPManagement.UI.GeneralClasses;
using Microsoft.Extensions.Localization;

namespace ERPManagement.UI.Components.Base.Services
{
    public interface IButtonActions<TModel>
    {
        // --- Actions ---
        Task NewEntity();
        Task EditEntity();
        Task DeleteEntity();
        Task SaveEntity();
        Task SaveAndCloseEntity();
        Task CancelEntity();

        // --- State & Config used by buttons UI ---
        FormState State { get; }
        HiddenButtonsConfig HiddenButtons { get; }
        bool IsArabic { get; }
        string FormName { get; }
        IStringLocalizer Localizer { get; }
    }
}
