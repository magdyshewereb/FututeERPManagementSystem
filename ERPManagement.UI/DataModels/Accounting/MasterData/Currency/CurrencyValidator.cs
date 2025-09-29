using ERPManagement.Application.Contracts.Infrastructure.Services;
using ERPManagement.Application.Shared.Constants;
using ERPManagement.Application.Shared.Services;

namespace ERPManagement.UI.DataModels.Accounting.MasterData.Currency
{
    public class CurrencyValidator : IValidator<Currency>
    {
        internal readonly IMessageLocalizationService _messageLocalization;
        public CurrencyValidator(IMessageLocalizationService messageLocalization)
        {
            _messageLocalization = messageLocalization;
        }
        public List<string> Validate(Currency currency)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(currency.CurrencyCode))
                errors.Add(_messageLocalization.GetMessage(ValidationMessages.RequitedCode));

            if (string.IsNullOrWhiteSpace(currency.CurrencyNameAr))
                errors.Add(_messageLocalization.GetMessage(ValidationMessages.RequiredNameAr));
            if (string.IsNullOrWhiteSpace(currency.CurrencyNameEn))
                errors.Add(_messageLocalization.GetMessage(ValidationMessages.RequiredNameEn));
            if (string.IsNullOrWhiteSpace(currency.ChangeNameAr))
                errors.Add(_messageLocalization.GetMessage(ValidationMessages.RequiredNameAr));
            if (string.IsNullOrWhiteSpace(currency.ChangeNameEn))
                errors.Add(_messageLocalization.GetMessage(ValidationMessages.RequiredNameEn));

            return errors;
        }
    }

}
