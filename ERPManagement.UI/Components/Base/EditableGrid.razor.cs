using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Security.AccessControl;

namespace ERPManagement.UI.Components.Base
{
    public partial class EditableGrid<TItem> : ComponentBase
    {
        [Parameter]
        public List<TItem> Items { get; set; }
        public TItem Item { get; set; }
        //[Parameter]
        //public TItem DefultItem { get; set; }
        [Parameter]
        public List<string> InvisibleColumns { get; set; }
        [Parameter]
        public DataSet dts { get; set; }
        [Parameter]
        public Dictionary<string, Dictionary<string, string>> valueLists { get; set; }
        [Parameter]
        public Dictionary<string, string> lstFilters { get; set; }
        [Parameter]
        public IStringLocalizer localizer { get; set; }
        [Parameter]
        public EventCallback<object> OnItemSelectedCallback { get; set; }
        [Parameter]
        public Func<TItem, TItem> OnAddNewItemFunc { get; set; }
        [Parameter]
        public Func<TItem,string, TItem> OnCellUpdateFunc { get; set; }
        [Parameter]
        public Func<TItem, string, TItem> OnCellListSelectFunc { get; set; }
        [Parameter]
        public int itemsPerPage { get; set; } = 5;
        int currentPage = 1;
        public bool isQMBModelVisible { get; set; }
        public string QMBMessage { get; set; }="";
        public string QMBTitle { get; set; } = "";
        int totalPages { get; set; }
        int counter { get; set; }
        [Parameter]
        public Dictionary<string, int> ColWidth { get; set; }
        private SystemSettings systemSettings { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            SelectFirstItemOfCurrentPage();
            systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();
        }
        protected override void OnParametersSet()
        {
            totalPages = (int)Math.Ceiling((double)Items.Count() / itemsPerPage);
            counter = (currentPage - 1) * itemsPerPage;
        }
        private void AddRow()
        {
            TItem item= Activator.CreateInstance<TItem>();
            if (OnAddNewItemFunc != null)
            {
                item = OnAddNewItemFunc.Invoke(item);
            }
            Items.Add(item);
            totalPages = (int)Math.Ceiling((double)Items.Count() / itemsPerPage);
            currentPage = totalPages;
        }
        private void ChangeItemsPerPage(ChangeEventArgs e)
        {
            itemsPerPage =int.Parse(e.Value.ToString());
            totalPages = (int)Math.Ceiling((double)Items.Count() / itemsPerPage);
        }
        private void SelectItem(TItem item)
        {
            if (item != null)
            { 
                Item = item;
                OnItemSelectedCallback.InvokeAsync(item); 
            }

        }
        private async Task btnDeleteRow(TItem item)
        {
            isQMBModelVisible = true;
            QMBMessage = systemSettings.IsArabic ? "هل تريد الحذف؟" : "Are you sure you want to delete?";
            QMBTitle = systemSettings.IsArabic ? "حذف" : "Delete";
        }
        private async Task DeleteRow()
        {
            if (SelectItem != null)
            {
                Items.Remove(Item);
            }
        }
        private async void ConfirmAction()
        {
            if (QMBTitle == "حذف" || QMBTitle == "Delete")
            {
               await DeleteRow();
            }
            isQMBModelVisible = false;
        }
        private async Task CancelAction()
        {
            isQMBModelVisible = false;
        }
        private void PreviousPage()
        {
            if (currentPage > 1)
            {
                currentPage--;
                SelectFirstItemOfCurrentPage();
            }
        }
        private void FirstPage()
        {

            currentPage = 1;
            SelectFirstItemOfCurrentPage();

        }
        private void NextPage()
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                SelectFirstItemOfCurrentPage();
            }
        }
        private void LastPage()
        {
            currentPage = totalPages;
            SelectFirstItemOfCurrentPage();

        }
        private void GoToPage(int pageNumber)
        {
            currentPage = pageNumber;
            SelectFirstItemOfCurrentPage();
        }
        private void SelectFirstItemOfCurrentPage()
        {
            var firstItemOfCurrentPage = Items.Skip((currentPage - 1) * itemsPerPage).FirstOrDefault();
            if (firstItemOfCurrentPage != null)
            {
                SelectItem(firstItemOfCurrentPage);
            }
        }
       
        private void UpdateModel(PropertyInfo property, object newValue,TItem item)
        {
            if (property.PropertyType == typeof(string))
            {
                property.SetValue(item, newValue);
            }
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
            {
                if (int.TryParse(newValue.ToString(), out var intValue))
                {
                    property.SetValue(item, intValue);
                }
            }
            else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
            {
                if (decimal.TryParse(newValue.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal decValue))
                {
                    property.SetValue(item, decValue);
                }
                item = OnCellUpdateFunc.Invoke(item, property.Name);
            }
            else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
            {
                if (DateTime.TryParse(newValue == null ? "" : newValue.ToString(), out var dateValue))
                {
                    property.SetValue(item, dateValue);
                }

            }
            else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
            {
                if (bool.TryParse(newValue.ToString(), out var boolValue))
                {
                    property.SetValue(item, boolValue);
                }
            }
        }

        private void CellListChanged(PropertyInfo property, object newValue, TItem item)
        {
            if (int.TryParse(newValue.ToString(), out var intValue) && OnCellListSelectFunc != null)
            {
                property.SetValue(item, intValue);
                item = OnCellListSelectFunc.Invoke(item, property.Name);
            }
        }
     
    }
}
