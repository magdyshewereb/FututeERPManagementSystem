using Microsoft.AspNetCore.Components;

namespace ERPManagement.UI.Components.Base
{
    public partial class DynamicTreeWithDetails<TModel> : ComponentBase

         where TModel : new()
    {

        [Parameter] public IEnumerable<TModel> Data { get; set; }
        [Parameter] public Func<TModel, object> GetId { get; set; }
        [Parameter] public Func<TModel, object> GetParentId { get; set; }
        [Parameter] public Func<TModel, string> GetText { get; set; }
        [Parameter] public RenderFragment<TModel> ChildContent { get; set; }
        [Parameter] public string TreeDirection { get; set; } = "rtl";

        private List<TreeNode> TreeData { get; set; }
        private TModel SelectedItem;
        private HashSet<object> Ancestors = new();
        private string searchTerm = "";

        protected override void OnParametersSet()
        {
            if (Data != null)
            {
                var lookup = Data.ToLookup(GetParentId);

                List<TreeNode> BuildBranch(object parentId)
                {
                    return lookup[parentId]
                        .Select(item => new TreeNode
                        {
                            Item = item,
                            Id = GetId(item),
                            ParentId = GetParentId(item),
                            Text = GetText(item),
                            Children = BuildBranch(GetId(item))
                        })
                        .ToList();
                }

                TreeData = BuildBranch(null);
            }
        }

        private void OnSearch(ChangeEventArgs e)
        {
            searchTerm = e.Value?.ToString() ?? "";
            ApplySearch(TreeData);
        }

        private void ApplySearch(IEnumerable<TreeNode> nodes)
        {
            foreach (var node in nodes)
            {
                // هل النص مطابق للبحث؟
                node.IsMatch = !string.IsNullOrWhiteSpace(searchTerm) &&
                               node.Text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);

                // نكمل على الأبناء
                ApplySearch(node.Children);

                // لو الأبناء فيهم نتائج → افتح الأب
                if (node.Children.Any(c => c.IsMatch || c.IsExpanded))
                {
                    node.IsExpanded = true;
                }
            }
        }

        private RenderFragment RenderTree() => builder =>
        {
            if (TreeData != null)
            {
                builder.OpenElement(0, "ul");
                builder.AddAttribute(1, "class", "tree-root");
                foreach (var root in TreeData)
                {
                    builder.AddContent(2, RenderNode(root));
                }
                builder.CloseElement();
            }
            else
            {
                builder.AddContent(3, (MarkupString)"<p class='text-muted'>جاري تحميل البيانات...</p>");
            }
        };

        private RenderFragment RenderNode(TreeNode node) => builder =>
        {
            builder.OpenElement(0, "li");

            var cssClass = "tree-node";
            if (Equals(node.Item, SelectedItem))
                cssClass += " selected";
            else if (Ancestors.Contains(node.Id))
                cssClass += " ancestor";
            if (node.IsMatch)
                cssClass += " highlight";

            if (node.Children.Any())
            {
                builder.OpenElement(1, "div");
                builder.AddAttribute(2, "class", cssClass);
                builder.AddAttribute(3, "onclick", EventCallback.Factory.Create(this, () => ToggleExpand(node)));

                builder.OpenElement(4, "i");
                builder.AddAttribute(5, "class", $"bi bi-chevron-right tree-icon {(node.IsExpanded ? "expanded" : "")}");
                builder.CloseElement();

                builder.AddContent(6, node.Text);
                builder.CloseElement();

                if (node.IsExpanded)
                {
                    builder.OpenElement(7, "ul");
                    foreach (var child in node.Children)
                    {
                        builder.AddContent(8, RenderNode(child));
                    }
                    builder.CloseElement();
                }
            }
            else
            {
                builder.OpenElement(9, "div");
                builder.AddAttribute(10, "class", cssClass);
                builder.AddAttribute(11, "onclick", EventCallback.Factory.Create(this, () => SelectNode(node)));

                builder.OpenElement(12, "i");
                builder.AddAttribute(13, "class", "bi bi-dot tree-icon");
                builder.CloseElement();

                builder.AddContent(14, node.Text);
                builder.CloseElement();
            }

            builder.CloseElement();
        };

        private RenderFragment RenderDetails() => builder =>
        {
            if (SelectedItem != null)
                builder.AddContent(0, ChildContent(SelectedItem));
            else
                builder.AddContent(1, (MarkupString)"<p class='text-muted'>اختر عنصر من الشجرة لعرض التفاصيل</p>");
        };

        private void ToggleExpand(TreeNode node) => node.IsExpanded = !node.IsExpanded;

        private void SelectNode(TreeNode node)
        {
            SelectedItem = (TModel)node.Item;
            Ancestors.Clear();
            MarkAncestors(TreeData, node.Id);
        }

        private bool MarkAncestors(IEnumerable<TreeNode> nodes, object targetId)
        {
            foreach (var node in nodes)
            {
                if (Equals(node.Id, targetId))
                    return true;

                if (MarkAncestors(node.Children, targetId))
                {
                    Ancestors.Add(node.Id);
                    return true;
                }
            }
            return false;
        }

        private class TreeNode
        {
            public object Id { get; set; }
            public object ParentId { get; set; }
            public string Text { get; set; }
            public TModel Item { get; set; }
            public List<TreeNode> Children { get; set; } = new();
            public bool IsExpanded { get; set; }
            public bool IsMatch { get; set; }  // للتظليل
        }
    }
}
