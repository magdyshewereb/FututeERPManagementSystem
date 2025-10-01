using ERPManagement.UI.Components.Base.Services.Grid;
using Microsoft.AspNetCore.Components;
using System.Data;

namespace ERPManagement.UI.Components.Base
{
    public partial class DynamicGrid<TModel> where TModel : new()
    {
        //[CascadingParameter] public PageLayoutComponent<TModel> GridHost { get; set; }
        [Parameter] public IGridHost<TModel> GridHost { get; set; }
        private string SearchText { get; set; } = string.Empty;
        //private List<string> SelectedSearchColumns { get; set; } = new();
        private List<string> SelectedSearchColumns = new(); // يبدأ فاضي

        private string SortColumn { get; set; }
        private bool SortAscending { get; set; } = true;

        private int _pageIndex = 0;
        private int _pageSize = 10;

        private IEnumerable<DataRow> FilteredData
        {
            get
            {
                if (GridHost?.Data == null) return Enumerable.Empty<DataRow>();
                var rows = GridHost.Data.AsEnumerable();
                if (string.IsNullOrWhiteSpace(SearchText)) return rows;
                var columnsToSearch = SelectedSearchColumns.Any()
                    ? SelectedSearchColumns
                    : GridHost.Data.Columns.Cast<DataColumn>().Select(c => c.ColumnName);
                return rows.Where(row => columnsToSearch.Any(col => row[col]?.ToString()?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true));
            }
        }
        private IEnumerable<DataRow> SortedData =>
            string.IsNullOrEmpty(SortColumn)
                ? FilteredData
                : (SortAscending ? FilteredData.OrderBy(r => r[SortColumn]) : FilteredData.OrderByDescending(r => r[SortColumn]));
        private IEnumerable<DataRow> PagedData => SortedData.Skip(_pageIndex * _pageSize).Take(_pageSize);
        private int TotalPages
        {
            get
            {
                var count = FilteredData.Count();
                if (_pageSize <= 0) return 1;
                return (int)Math.Ceiling((double)count / _pageSize);
            }
        }
        private void Sort(string column)
        {
            if (SortColumn == column)
                SortAscending = !SortAscending;
            else
            {
                SortColumn = column;
                SortAscending = true;
            }
        }
        private void PrevPage()
        {
            if (_pageIndex > 0) _pageIndex--;
        }
        private void NextPage()
        {
            if (_pageIndex < TotalPages - 1) _pageIndex++;
        }
        private void FirstPage()
        {
            _pageIndex = 0;
        }
        private void LastPage()
        {
            _pageIndex = TotalPages - 1;
        }
        private void GoToPage(int pageIndex)
        {
            if (pageIndex >= 0 && pageIndex < TotalPages)
                _pageIndex = pageIndex;
        }
        private void OnPageSizeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e?.Value?.ToString(), out var newSize) && newSize > 0)
                _pageSize = newSize;
            else
                _pageSize = 10;

            _pageIndex = 0;
        }
        private string GetDisplayValue(DataRow row, DataColumn col)
        {
            if (row == null || col == null) return string.Empty;

            var colName = col.ColumnName;
            var cell = row[colName];

            if (cell == null || cell == DBNull.Value)
                return string.Empty;

            var fks = GridHost?.ForeignKeyLookups;
            if (fks != null && fks.TryGetValue(colName, out var map) && map != null)
            {
                if (map.TryGetValue(cell, out var display1) && !string.IsNullOrEmpty(display1))
                    return display1;

                var s = cell.ToString();
                if (map.TryGetValue(s, out var display2) && !string.IsNullOrEmpty(display2))
                    return display2;

                if (int.TryParse(s, out var intKey) && map.TryGetValue(intKey, out var display3) && !string.IsNullOrEmpty(display3))
                    return display3;

                if (long.TryParse(s, out var longKey) && map.TryGetValue(longKey, out var display4) && !string.IsNullOrEmpty(display4))
                    return display4;
            }

            return cell.ToString();
        }
        protected override void OnParametersSet()
        {
            if (GridHost?.Data != null && GridHost.Data.Columns.Count > 0)
            {
                if (!SelectedSearchColumns.Any())
                {
                    SelectedSearchColumns = GridHost.Data.Columns.Cast<DataColumn>()
                        .Select(c => c.ColumnName)
                        .Except(GridHost.InvisibleColumns ?? new List<string>())
                        .ToList();
                }
            }
        }
        // row navigation & selection
        private int _selectedRowIndex = -1;
        private async Task SelectRowByIndex(int index)
        {
            var rows = PagedData.ToList();
            if (index >= 0 && index < rows.Count)
            {
                _selectedRowIndex = index;
                await SelectRow(rows[index]);
            }
        }
        public async Task SelectFirstItem() => await SelectRowByIndex(0);
        public async Task SelectLastItem()
        {
            var rows = PagedData.ToList();
            if (rows.Count > 0) await SelectRowByIndex(rows.Count - 1);
        }
        public async Task SelectNextItem()
        {
            var rows = PagedData.ToList();
            if (_selectedRowIndex >= 0 && _selectedRowIndex < rows.Count - 1) await SelectRowByIndex(_selectedRowIndex + 1);
        }
        public async Task SelectPreviousItem()
        {
            if (_selectedRowIndex > 0) await SelectRowByIndex(_selectedRowIndex - 1);
        }
        private async Task SelectRow(DataRow row)
        {
            var rows = PagedData.ToList();
            _selectedRowIndex = rows.IndexOf(row);
            if (GridHost != null)
            {
                GridHost.SelectedRow = row;
                await GridHost.RowSelected(row);
            }

        }

    }

    // Non-generic partial for localization resources
    public partial class DynamicGrid { }
}
