using ERPManagement.Application.Shared.Enums;
using ERPManagement.UI.GeneralClasses;
using Microsoft.Extensions.Localization;
using System.Data;

namespace ERPManagement.UI.Components.Base.Services
{
    public interface IEntityForm<TModel>
    {
        FormState State { get; set; }
        bool IsEnabled { get; set; }
        string FormName { get; }
        bool IsArabic { get; }
        HiddenButtonsConfig HiddenButtons { get; }

        TModel CurrentObject { get; set; }
        DataRow SelectedRow { get; set; }

        Action Refresh { get; }
        Func<TModel, int>? OnInsert { get; }
        Action<TModel>? OnUpdate { get; }
        Func<TModel, bool>? OnDelete { get; }

        DataTable? Data { get; set; }
        IStringLocalizer Localizer { get; }
    }

}
