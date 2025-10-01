namespace ERPManagement.UI.Components.Base.Services.Buttons.Navigations
{
    public interface IButtonNavigations<TModel>
    {
        Task FirstEntity();
        Task PreviousEntity();
        Task NextEntity();
        Task LastEntity();
    }
}
