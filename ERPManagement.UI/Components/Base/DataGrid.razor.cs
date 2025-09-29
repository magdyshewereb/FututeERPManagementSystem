using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Security.AccessControl;

namespace ERPManagement.UI.Components.Base
{
    public partial class DataGrid<TItem> : ComponentBase
    {
        [Parameter]
        public List<TItem> Items { get; set; }
        [Parameter]
        public List<string> InvisibleColumns { get; set; }

        [Parameter]
        public Dictionary<string, Dictionary<string, string>> valueLists { get; set; }

        [Parameter]
        public bool isNaveMode { get; set; } = false;
        [Parameter]
        public IStringLocalizer localizer { get; set; }
        [Parameter]
        public EventCallback<object> OnItemSelectedCallback { get; set; }

        [Parameter]
        public int itemsPerPage { get; set; } = 5;
        int selectItemIndex = -1;
        int currentPage = 1;
        TItem selectedItem { get; set; }
        int totalPages { get; set; }
        //int counter { get; set; }
        protected override void OnInitialized()
        {
            //base.OnInitialized();
            SelectFirstItemOfCurrentPage();
        }
        protected override void OnParametersSet()
        {
            totalPages = (int)Math.Ceiling((double)Items.Count() / itemsPerPage);
            //counter = (currentPage - 1) * itemsPerPage;
        
        }

        public void SelectNextItem()
        {
            if (selectItemIndex < Items.Count - 1)
            {
                selectItemIndex++;
                SelectItem(Items.ElementAt(selectItemIndex));
                if ((selectItemIndex + 1) % itemsPerPage == 1)
                {
                    //if (currentPage < totalPages)
                    //{
                        currentPage++;
                    //}
                }

            }
        }
        public void SelectPreviousItem()
        {
            if (selectItemIndex > 0)
            {
                selectItemIndex--;               
                if ((selectItemIndex + 1) % itemsPerPage == 0)
                {
                    //if (currentPage > 1)
                    //{
                        currentPage--;
                    //}
                }
                SelectItem(Items.ElementAt(selectItemIndex));
            }
        }
        private void ChangeItemsPerPage(ChangeEventArgs e)
        {
            itemsPerPage = int.Parse(e.Value.ToString());
            totalPages = (int)Math.Ceiling((double)Items.Count() / itemsPerPage);
            GoToPage(1);
        }
        private void SelectItem(object item)
        {
            if (isNaveMode && item is TItem)
            {
                selectedItem = (TItem)item;
                selectItemIndex = Items.IndexOf(selectedItem);
                OnItemSelectedCallback.InvokeAsync(item);
            }
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
                selectItemIndex = Items.IndexOf(firstItemOfCurrentPage);
                SelectItem(firstItemOfCurrentPage);
            }
        }
    }
}
