using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Data;

namespace ERPManagement.UI.Components.Base
{
    public partial class BaseGrid
    {
        [Parameter] public DataTable Data { get; set; }
        [Parameter] public List<string> InvisibleColumns { get; set; } = new();
        [Parameter] public int PageSize { get; set; } = 5;
        [Parameter] public IStringLocalizer localizer { get; set; }

        private DataTable PagedData = new();
        private string searchText = "";
        private string sortColumn = "";
        private bool sortAscending = true;
        private int currentPage = 1;
        private int TotalPages => (int)Math.Ceiling((double)FilteredData.Rows.Count / PageSize);

        private DataTable FilteredData => GetFilteredAndSortedData();

        protected override void OnParametersSet()
        {
            ApplyPagination();
        }

        private void SortBy(string column)
        {
            if (sortColumn == column)
                sortAscending = !sortAscending;
            else
            {
                sortColumn = column;
                sortAscending = true;
            }
            currentPage = 1;
            ApplyPagination();
        }

        private DataTable GetFilteredAndSortedData()
        {
            var dt = Data.Clone();
            IEnumerable<DataRow> rows = Data.AsEnumerable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                rows = rows.Where(r => r.ItemArray.Any(
                    v => v?.ToString()?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true));
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortColumn))
            {
                rows = sortAscending
                    ? rows.OrderBy(r => r[sortColumn])
                    : rows.OrderByDescending(r => r[sortColumn]);
            }

            foreach (var row in rows) dt.ImportRow(row);
            return dt;
        }

        private void ApplyPagination()
        {
            var dt = FilteredData.Clone();

            var rows = FilteredData.AsEnumerable()
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize);

            foreach (var row in rows) dt.ImportRow(row);

            PagedData = dt;
            StateHasChanged();
        }

        private void ChangePage(int page)
        {
            if (page >= 1 && page <= TotalPages)
            {
                currentPage = page;
                ApplyPagination();
            }
        }
    }
}
