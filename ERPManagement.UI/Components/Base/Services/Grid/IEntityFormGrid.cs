using ERPManagement.Application.Shared.Enums;
using Microsoft.Extensions.Localization;
using System.Data;

namespace ERPManagement.UI.Components.Base.Services
{
    public interface IEntityFormGrid<TModel>
    {
        FormState State { get; set; }
        DataTable? Data { get; set; }
        DataRow SelectedRow { get; set; }
        List<string> InvisibleColumns { get; set; }
        Dictionary<string, Dictionary<object, string>> ForeignKeyLookups { get; set; }

        bool IsArabic { get; }
        IStringLocalizer Localizer { get; }

        TModel CurrentObject { get; set; }
        TModel OldObject { get; set; }

        Func<DataRow, TModel> MapRowToModel { get; }
        Action Refresh { get; }
        //Task RowSelected(DataRow row);
    }
}
