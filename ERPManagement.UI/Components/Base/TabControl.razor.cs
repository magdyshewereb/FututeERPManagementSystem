using Microsoft.AspNetCore.Components;

namespace ERPManagement.UI.Components.Base
{
    public partial class TabControl : ComponentBase
    {
        // Next line is needed so we are able to add <TabPage> components inside
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public EventCallback <TabPage> OnTabClickCallback { get; set; }
        public TabPage ActivePage { get; set; }
        List<TabPage> Pages = new List<TabPage>();
        internal void RemovePage(TabPage tabPage)
        {
            if (Pages.Where(t=>t.Text == tabPage.Text).ToList().Count() > 0)
            {
                Pages.Remove(Pages.Where(t => t.Text == tabPage.Text).First());
            }
            if (Pages.Count > 0)
                ActivePage = Pages[0];
            StateHasChanged();
        }
        internal void AddPage(TabPage tabPage)
        {
            if (!Pages.Contains(tabPage))
            {
                Pages.Add(tabPage);
            }
            if (Pages.Count == 1)
                ActivePage = tabPage;
            StateHasChanged();
        }
        string GetButtonClass(TabPage page)
        {
            return page == ActivePage ? "active" : "btn-secondary";
        }
        void ActivatePage(TabPage page)
        {
            ActivePage = page;
            OnTabClickCallback.InvokeAsync(page);
        }
    }
}
