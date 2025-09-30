namespace ERPManagement.UI.Components.Base.Services.Buttons
{
    public interface IButtonNavigations<TModel>
    {
        Task FirstEntity();
        Task PreviousEntity();
        Task NextEntity();
        Task LastEntity();
    }
}
