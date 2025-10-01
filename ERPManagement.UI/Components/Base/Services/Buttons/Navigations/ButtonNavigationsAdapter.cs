using System.Data;

namespace ERPManagement.UI.Components.Base.Services.Buttons.Navigations
{
    public class ButtonNavigationsAdapter<TModel> : IButtonNavigations<TModel>
        where TModel : new()
    {
        private readonly IEntityFormNavigation<TModel> _form;

        public ButtonNavigationsAdapter(IEntityFormNavigation<TModel> form)
        {
            _form = form;
        }

        public Task FirstEntity()
        {
            if (_form.Data == null || _form.Data.Rows.Count == 0)
                return Task.CompletedTask;

            var firstRow = _form.Data.Rows.Cast<DataRow>().FirstOrDefault();
            if (firstRow != null && _form.MapRowToModel != null)
            {
                _form.CurrentObject = _form.MapRowToModel(firstRow);
                _form.OldObject = (TModel)(_form.CurrentObject as ICloneable)?.Clone() ?? _form.CurrentObject;
                _form.SelectedRow = firstRow;
                _form.Refresh();
            }
            return Task.CompletedTask;
        }

        public Task PreviousEntity()
        {
            if (_form.Data == null || _form.SelectedRow == null)
                return Task.CompletedTask;

            var rows = _form.Data.Rows.Cast<DataRow>().ToList();
            var index = rows.IndexOf(_form.SelectedRow);

            if (index > 0)
            {
                var prevRow = rows[index - 1];
                if (_form.MapRowToModel != null)
                {
                    _form.CurrentObject = _form.MapRowToModel(prevRow);
                    _form.OldObject = (TModel)(_form.CurrentObject as ICloneable)?.Clone() ?? _form.CurrentObject;
                    _form.SelectedRow = prevRow;
                    _form.Refresh();
                }
            }
            return Task.CompletedTask;
        }

        public Task NextEntity()
        {
            if (_form.Data == null || _form.SelectedRow == null)
                return Task.CompletedTask;

            var rows = _form.Data.Rows.Cast<DataRow>().ToList();
            var index = rows.IndexOf(_form.SelectedRow);

            if (index < rows.Count - 1)
            {
                var nextRow = rows[index + 1];
                if (_form.MapRowToModel != null)
                {
                    _form.CurrentObject = _form.MapRowToModel(nextRow);
                    _form.OldObject = (TModel)(_form.CurrentObject as ICloneable)?.Clone() ?? _form.CurrentObject;
                    _form.SelectedRow = nextRow;
                    _form.Refresh();
                }
            }
            return Task.CompletedTask;
        }

        public Task LastEntity()
        {
            if (_form.Data == null || _form.Data.Rows.Count == 0)
                return Task.CompletedTask;

            var lastRow = _form.Data.Rows.Cast<DataRow>().LastOrDefault();
            if (lastRow != null && _form.MapRowToModel != null)
            {
                _form.CurrentObject = _form.MapRowToModel(lastRow);
                _form.OldObject = (TModel)(_form.CurrentObject as ICloneable)?.Clone() ?? _form.CurrentObject;
                _form.SelectedRow = lastRow;
                _form.Refresh();
            }
            return Task.CompletedTask;
        }
    }
}
