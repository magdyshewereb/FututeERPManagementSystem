using ERPManagement.UI.DataModels.Accounting.MasterData.CostCenter;
using System.Data;

namespace ERPManagement.UI.Components.Base.Services.Tree
{
    public class TreeHostAdapter<TModel> : ITreeHost<TModel>
        where TModel : new()
    {
        private readonly TreeLayoutComponent<TModel> _layout;
        private ITreeHost<CostCenter>? treeHost;

        public TreeHostAdapter(TreeLayoutComponent<TModel> layout)
        {
            _layout = layout;
            InvisibleColumns = new List<string>();
            SelectedColumns = new List<string>();
        }

        public TreeHostAdapter(ITreeHost<CostCenter>? treeHost)
        {
            this.treeHost = treeHost;
        }

        public DataTable? Data { get; set; }

        public TModel? SelectedNode { get; set; }

        public bool IsArabic { get; set; }

        public List<string> InvisibleColumns { get; private set; }

        public List<string> SelectedColumns { get; set; }

        public void Refresh() => _layout.Refresh();
    }
}
