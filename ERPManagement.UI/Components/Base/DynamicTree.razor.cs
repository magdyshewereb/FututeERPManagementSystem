using Microsoft.AspNetCore.Components;

namespace ERPManagement.UI.Components.Base
{
    public partial class DynamicTree<TModel> : ComponentBase where TModel : class
    {
        [Parameter] public IEnumerable<TModel> Data { get; set; }
        [Parameter] public Func<TModel, object> GetId { get; set; }
        [Parameter] public Func<TModel, object> GetParentId { get; set; }
        [Parameter] public Func<TModel, string> GetText { get; set; }
        [Parameter] public EventCallback<TModel> OnNodeClick { get; set; }

        public List<TreeNode> TreeNodes { get; set; }

        protected override void OnParametersSet()
        {
            if (Data != null)
            {
                TreeNodes = Data.Select(item => new TreeNode
                {
                    Item = item,
                    Id = GetId(item),
                    ParentId = GetParentId(item),
                    Text = GetText(item)
                }).ToList();

                foreach (var node in TreeNodes)
                {
                    node.Children = TreeNodes.Where(n => Equals(n.ParentId, node.Id)).ToList();
                }
            }
        }

        public void HandleNodeClick(TreeNode node)
        {
            OnNodeClick.InvokeAsync(node.Item);
        }

        public class TreeNode
        {
            public object Id { get; set; }
            public object ParentId { get; set; }
            public string Text { get; set; }
            public TModel Item { get; set; }
            public List<TreeNode> Children { get; set; }
        }
    }
}
