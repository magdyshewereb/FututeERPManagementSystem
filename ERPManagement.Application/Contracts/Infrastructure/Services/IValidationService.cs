namespace ERPManagement.Application.Contracts.Infrastructure.Services
{
    public interface IValidationService<T>
    {
        Task<List<string>> ValidateAsync(T model);
    }
}
