using System.Data;

namespace ERPManagement.UI.Components.Base.Services.Tree
{
    public interface ITreeHost<TModel>
        where TModel : new()
    {
        // البيانات اللى هتتربط بالشجرة
        DataTable? Data { get; set; }

        // العنصر المختار من الشجرة
        TModel? SelectedNode { get; set; }

        // لعرض حالة اللغة (RTL/LTR)
        bool IsArabic { get; set; }

        // أعمدة مخفية
        List<string> InvisibleColumns { get; }

        // أعمدة مختارة للعرض
        List<string> SelectedColumns { get; set; }

        // تحديث الواجهة
        void Refresh();
    }
}
