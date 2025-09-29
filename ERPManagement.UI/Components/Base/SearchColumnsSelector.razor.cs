using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace ERPManagement.UI.Components.Base
{
    public partial class SearchColumnsSelector<TModel> : ComponentBase where TModel : new()
    {
        [CascadingParameter] public PageLayoutComponent<TModel> BasePage { get; set; }
        [Parameter] public List<string> Columns { get; set; } = new();
        [Parameter] public List<string> InvisibleColumns { get; set; } = new();
        [Parameter] public List<string> SelectedColumns { get; set; } = new();
        [Parameter] public EventCallback<List<string>> SelectedColumnsChanged { get; set; }
        [Parameter] public IStringLocalizer Localizer { get; set; }

        private string SearchText { get; set; } = string.Empty;
        private bool IsDropdownOpen { get; set; }

        private List<string> FilteredColumns =>
            Columns.Where(c => string.IsNullOrEmpty(SearchText) ||
                               c.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                   .ToList();

        private void ToggleDropdown() => IsDropdownOpen = !IsDropdownOpen;

        private async Task ToggleColumn(string col)
        {
            if (SelectedColumns.Contains(col))
                SelectedColumns.Remove(col);
            else
                SelectedColumns.Add(col);

            await SelectedColumnsChanged.InvokeAsync(SelectedColumns);
        }

        private async Task RemoveColumn(string col)
        {
            if (SelectedColumns.Contains(col))
            {
                SelectedColumns.Remove(col);
                await SelectedColumnsChanged.InvokeAsync(SelectedColumns);
            }
        }

        private async Task SelectAll()
        {
            SelectedColumns = Columns.Except(BasePage.InvisibleColumns).ToList();
            await SelectedColumnsChanged.InvokeAsync(SelectedColumns);
        }

        private async Task UnselectAll()
        {
            SelectedColumns.Clear();
            await SelectedColumnsChanged.InvokeAsync(SelectedColumns);
        }
    }
    // Non-generic partial class so IStringLocalizer<DynamicButtons> can bind to resources
    public partial class SearchColumnsSelector { }
}
