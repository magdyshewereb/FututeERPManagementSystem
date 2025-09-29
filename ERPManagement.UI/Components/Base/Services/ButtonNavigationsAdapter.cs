using ERPManagement.UI.Components.Base.Services;
using System.Data;

namespace ERPManagement.UI.Components.Base
{
    public class ButtonNavigationsAdapter<TModel> : IButtonNavigations<TModel>
        where TModel : new()
    {
        private readonly PageLayoutComponent<TModel> _page;

        public ButtonNavigationsAdapter(PageLayoutComponent<TModel> page)
        {
            _page = page;
        }

        public Task FirstEntity()
        {
            if (_page.Data == null || _page.Data.Rows.Count == 0) return Task.CompletedTask;
            var firstRow = _page.Data.Rows.Cast<DataRow>().FirstOrDefault();
            if (firstRow != null) return _page.OnRowSelected.InvokeAsync(firstRow);
            return Task.CompletedTask;
        }

        public Task PreviousEntity()
        {
            if (_page.Data == null || _page.SelectedRow == null) return Task.CompletedTask;

            var rows = _page.Data.Rows.Cast<DataRow>().ToList();
            var index = rows.IndexOf(_page.SelectedRow);

            if (index > 0)
            {
                var prevRow = rows[index - 1];
                return _page.OnRowSelected.InvokeAsync(prevRow);
            }
            return Task.CompletedTask;
        }

        public Task NextEntity()
        {
            if (_page.Data == null || _page.SelectedRow == null) return Task.CompletedTask;

            var rows = _page.Data.Rows.Cast<DataRow>().ToList();
            var index = rows.IndexOf(_page.SelectedRow);

            if (index < rows.Count - 1)
            {
                var nextRow = rows[index + 1];
                return _page.OnRowSelected.InvokeAsync(nextRow);
            }
            return Task.CompletedTask;
        }

        public Task LastEntity()
        {
            if (_page.Data == null || _page.Data.Rows.Count == 0) return Task.CompletedTask;
            var lastRow = _page.Data.Rows.Cast<DataRow>().LastOrDefault();
            if (lastRow != null) return _page.OnRowSelected.InvokeAsync(lastRow);
            return Task.CompletedTask;
        }
    }
}
