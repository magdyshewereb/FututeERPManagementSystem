using ERPManagement.Application.Contracts.Infrastructure.Services;
using Microsoft.JSInterop;

namespace ERPManagement.Infrastructure.Services
{
    public class SweetAlertNotificationService : INotificationService
    {
        private readonly IJSRuntime _js;

        public SweetAlertNotificationService(IJSRuntime js)
        {
            _js = js;
        }

        public Task Success(string message) =>
            _js.InvokeVoidAsync("Swal.fire", "نجاح", message, "success").AsTask();

        public Task Error(string message) =>
            _js.InvokeVoidAsync("Swal.fire", "خطأ", message, "error").AsTask();

        public Task Warning(string message) =>
            _js.InvokeVoidAsync("Swal.fire", "تنبيه", message, "warning").AsTask();

        public async Task Confirm(string message, Func<Task> onConfirm)
        {
            var result = await _js.InvokeAsync<bool>("Swal.fire", new
            {
                title = "تأكيد",
                text = message,
                icon = "question",
                showCancelButton = true
            });

            if (result && onConfirm != null)
                await onConfirm();
        }
    }
}
