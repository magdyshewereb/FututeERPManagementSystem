using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace ERPManagement.UI.Components.Base
{
    public partial class SearchColumnsSelector<TModel> : ComponentBase where TModel : new()
    {
        [Parameter] public List<string> DisplayedColumns { get; set; } = new();
        [Parameter] public List<string> InvisibleColumns { get; set; } = new();
        [Parameter] public List<string> SelectedColumns { get; set; } = new();
        [Parameter] public EventCallback<List<string>> SelectedColumnsChanged { get; set; }
        [Parameter] public IStringLocalizer Localizer { get; set; }
        [Parameter] public bool IsArabic { get; set; }

        private string SearchText { get; set; } = string.Empty;
        private bool IsDropdownOpen { get; set; }

        private ElementReference DropdownElement;
        private IJSObjectReference? _module;
        private IJSObjectReference? _outsideClickHandler;

        [Inject] private IJSRuntime JS { get; set; }


        //private List<string> FilteredColumns =>
        // DisplayedColumns
        //.Where(c => !InvisibleColumns.Contains(c)) // استبعاد الأعمدة الغير مرئية
        //.Where(c => string.IsNullOrEmpty(SearchText) ||
        //            (Localizer?[c]?.Value ?? c)
        //                .Contains(SearchText, StringComparison.OrdinalIgnoreCase)) // البحث في الاسم المعروض
        //.ToList();

        private List<string> FilteredColumns =>
    DisplayedColumns
        .Where(c => !InvisibleColumns.Contains(c)) // استبعاد الأعمدة الغير مرئية
        .Where(c =>
        {
            var displayName = Localizer?[c]?.Value ?? c; // الاسم المعروض
            return string.IsNullOrEmpty(SearchText) ||
                   displayName.IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) >= 0;
        })
        .ToList();

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
            if (SelectedColumns.Count > 1)
            {
                if (SelectedColumns.Contains(col))
                {
                    SelectedColumns.Remove(col);
                    await SelectedColumnsChanged.InvokeAsync(SelectedColumns);
                }
            }
        }

        private async Task SelectAll()
        {
            SelectedColumns = DisplayedColumns.Except(InvisibleColumns).ToList();
            await SelectedColumnsChanged.InvokeAsync(SelectedColumns);
        }

        private async Task UnselectAll()
        {
            SelectedColumns.Clear();
            await SelectedColumnsChanged.InvokeAsync(SelectedColumns);
        }

        private async Task ToggleDropdown()
        {
            IsDropdownOpen = !IsDropdownOpen;

            if (IsDropdownOpen)
            {
                _module ??= await JS.InvokeAsync<IJSObjectReference>("import", "/js/dropdownHelper.js");

                _outsideClickHandler = await _module.InvokeAsync<IJSObjectReference>(
                    "registerOutsideClick", DropdownElement, DotNetObjectReference.Create(this));

            }
            else
            {
                if (_outsideClickHandler is not null)
                {
                    await _outsideClickHandler.InvokeVoidAsync("dispose");
                    _outsideClickHandler = null;
                }
            }
        }

        [JSInvokable]
        public Task CloseDropdown()
        {
            IsDropdownOpen = false;
            StateHasChanged();
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_outsideClickHandler is not null)
            {
                await _outsideClickHandler.InvokeVoidAsync("dispose");
            }
        }
    }

    public partial class SearchColumnsSelector { }
}
