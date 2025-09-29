using Microsoft.Extensions.Localization;
using System.Data;

namespace ERPManagement.UI.Components.Base.Services
{
    public interface IGridHost<TModel>
    {
        DataTable Data { get; }
        List<string> InvisibleColumns { get; }
        Dictionary<string, Dictionary<object, string>> ForeignKeyLookups { get; }
        DataRow SelectedRow { get; set; }
        Task RowSelected(DataRow row);
        bool IsArabic { get; }
        IStringLocalizer Localizer { get; }
    }
}
