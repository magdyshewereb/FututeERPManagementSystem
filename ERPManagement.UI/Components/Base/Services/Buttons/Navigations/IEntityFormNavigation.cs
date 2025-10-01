using System.Data;

namespace ERPManagement.UI.Components.Base.Services
{
    public interface IEntityFormNavigation<TModel>
    {
        DataTable? Data { get; set; }
        public Func<DataRow, TModel> MapRowToModel { get; }
        TModel CurrentObject { get; set; }
        TModel OldObject { get; set; }
        DataRow SelectedRow { get; set; }
        //Task RowSelected(DataRow row);
        Action Refresh { get; }
    }
}
