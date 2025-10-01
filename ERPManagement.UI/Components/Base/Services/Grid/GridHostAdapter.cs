using ERPManagement.Application.Shared.Enums;
using Microsoft.Extensions.Localization;
using System.Data;

namespace ERPManagement.UI.Components.Base.Services.Grid
{
    public class GridHostAdapter<TModel> : IGridHost<TModel>
        where TModel : new()
    {
        private readonly IEntityFormGrid<TModel> _form;

        public GridHostAdapter(IEntityFormGrid<TModel> form)
        {
            _form = form ?? throw new ArgumentNullException(nameof(form));
        }

        public DataTable Data => _form.Data;
        public List<string> InvisibleColumns => _form.InvisibleColumns;
        public Dictionary<string, Dictionary<object, string>> ForeignKeyLookups => _form.ForeignKeyLookups;
        public DataRow SelectedRow
        {
            get => _form.SelectedRow;
            set => _form.SelectedRow = value;
        }
        public bool IsArabic => _form.IsArabic;
        public IStringLocalizer Localizer => _form.Localizer;

        public async Task RowSelected(DataRow row)
        {
            _form.State = FormState.View;

            if (row != null)
            {
                if (_form.MapRowToModel != null)
                    _form.CurrentObject = _form.MapRowToModel(row);

                _form.OldObject = (TModel)(_form.CurrentObject as ICloneable)?.Clone() ?? _form.CurrentObject;
            }

            _form.Refresh();
        }
    }
}
