using Microsoft.AspNetCore.Components;

namespace ERPManagement.UI.Components.Base
{
    public partial class TabPage :ComponentBase
    {
        [CascadingParameter]
        private TabControl Parent { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Text { get; set; }
        [Parameter]
        public bool IsAdded { get; set; }
        protected override void OnInitialized()
        {
            if (Parent == null)
                throw new ArgumentNullException(nameof(Parent), "TabPage must exist within a TabControl");
            if (IsAdded)
            {
                Parent.AddPage(this);
            }
            else
            {
                Parent.RemovePage(this);
            }
            base.OnInitialized();
        }

    }
}
