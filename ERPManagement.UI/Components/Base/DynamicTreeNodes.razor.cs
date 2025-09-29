using Microsoft.AspNetCore.Components;

namespace ERPManagement.UI.Components.Base
{
    public partial class DynamicTreeNodes<TModel> : ComponentBase where TModel : class
    {
        [Parameter] public List<DynamicTree<TModel>.TreeNode> Nodes { get; set; }
        [Parameter] public EventCallback<TModel> OnNodeClick { get; set; }

        void HandleNodeClick(DynamicTree<TModel>.TreeNode node)
        {
            OnNodeClick.InvokeAsync(node.Item);
        }
    }
}
