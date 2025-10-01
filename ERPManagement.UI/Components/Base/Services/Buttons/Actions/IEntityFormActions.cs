using ERPManagement.Application.Shared.Enums;
using ERPManagement.UI.GeneralClasses;
using Microsoft.Extensions.Localization;
using System.Data;

namespace ERPManagement.UI.Components.Base.Services
{
    public interface IEntityFormActions<TModel>
    {
        FormState State { get; set; }
        bool IsEnabled { get; set; }
        bool IsArabic { get; set; }
        string FormName { get; set; }
        HiddenButtonsConfig HiddenButtons { get; set; }
        public Func<DataRow, TModel> MapRowToModel { get; }
        DataTable? Data { get; set; }
        TModel CurrentObject { get; set; }
        TModel OldObject { get; set; }
        DataRow SelectedRow { get; set; }

        Action Refresh { get; }

        Func<TModel, int>? OnInsert { get; }
        Func<TModel, bool>? OnUpdate { get; }
        Func<TModel, bool>? OnDelete { get; }
        Func<TModel, string>? CheckBeforeDelete { get; }

        IStringLocalizer Localizer { get; }
        List<string> ValidationErrors { get; set; }
        int CurrentBranchID { get; set; }
    }
}
