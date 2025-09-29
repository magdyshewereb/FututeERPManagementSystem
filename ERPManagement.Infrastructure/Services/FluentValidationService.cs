using ERPManagement.Application.Contracts.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace ERPManagement.Infrastructure.Services
{
    public class FluentValidationService<T> : IValidationService<T>
    {
        private readonly FluentValidation.IValidator<T> _validator;
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        public FluentValidationService(FluentValidation.IValidator<T> validator)
        {
            _validator = validator;
        }

        public async Task<List<string>> ValidateAsync(T model)
        {
            var result = await _validator.ValidateAsync(model);
            return result.IsValid
                ? new List<string>()
                : result.Errors.Select(e => e.ErrorMessage).ToList();


        }
    }
}
