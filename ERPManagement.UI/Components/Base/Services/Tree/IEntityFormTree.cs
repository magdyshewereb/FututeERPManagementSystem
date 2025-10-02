namespace ERPManagement.UI.Components.Base.Services.Tree
{
    public interface IEntityFormTree<TModel>
        where TModel : new()
    {
        // العقدة (Node) الحالية المختارة
        TModel CurrentObject { get; set; }

        // العقدة القديمة (قبل التعديل)
        TModel OldObject { get; set; }

        // الأحداث الأساسية
        Func<TModel, int>? OnInsert { get; set; }
        Func<TModel, bool>? OnUpdate { get; set; }
        Func<TModel, bool>? OnDelete { get; set; }
        Func<TModel, string>? CheckBeforeDelete { get; set; }
    }
}
