namespace ERPManagement.Application.Contracts.Infrastructure.Services
{
    public interface INotificationService
    {
        Task Success(string message);
        Task Error(string message);
        Task Warning(string message);
        Task Confirm(string message, Func<Task> onConfirm);
    }
}
