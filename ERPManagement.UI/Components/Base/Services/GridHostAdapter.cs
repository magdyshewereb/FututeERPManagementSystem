using ERPManagement.Application.Shared.Enums;
using Microsoft.Extensions.Localization;
using System.Data;

namespace ERPManagement.UI.Components.Base.Services
{
    public class GridHostAdapter<TModel> : IGridHost<TModel>
        where TModel : new()
    {
        private readonly PageLayoutComponent<TModel> _page;
        public GridHostAdapter(PageLayoutComponent<TModel> page)
        {
            _page = page ?? throw new ArgumentNullException(nameof(page));
        }

        public DataTable Data => _page.Data;
        public List<string> InvisibleColumns => _page.InvisibleColumns;
        public Dictionary<string, Dictionary<object, string>> ForeignKeyLookups => _page.ForeignKeyLookups;
        public DataRow SelectedRow
        {
            get => _page.SelectedRow;
            set => _page.SelectedRow = value;
        }
        public bool IsArabic => _page.IsArabic;
        public IStringLocalizer Localizer => _page.Localizer;

        //public Task RowSelected(DataRow row)
        //{
        //    _page.State = FormState.View;
        //    if (row != null)
        //    {
        //        if (_page.MapRowToModel != null)
        //            _page.CurrentObject = _page.MapRowToModel(row);
        //        _page.OldObject = (TModel)(_page.CurrentObject as ICloneable)?.Clone() ?? _page.CurrentObject;
        //    }

        //    return Task.CompletedTask;
        //}
        public Task RowSelected(DataRow row)
        {
            _page.SelectedRow = row; // لازم تحفظ الـ Row المختار
            _page.State = FormState.View;

            if (row != null)
            {
                if (_page.MapRowToModel != null)
                    _page.CurrentObject = _page.MapRowToModel(row);

                _page.OldObject = (_page.CurrentObject as ICloneable)?.Clone() is TModel clone
                    ? clone
                    : _page.CurrentObject;


            }
            //InvokeAsync(StateHasChanged);
            return _page.OnRowSelected.HasDelegate
       ? _page.OnRowSelected.InvokeAsync(row)
       : Task.CompletedTask;
        }


    }
}
