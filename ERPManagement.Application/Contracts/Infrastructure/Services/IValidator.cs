namespace ERPManagement.Application.Contracts.Infrastructure.Services
{
    public interface IValidator<T>
    {
        List<string> Validate(T entity);
    }
}
