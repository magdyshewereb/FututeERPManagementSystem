using ERPManagement.Application.Contracts.Infrastructure.Services;
using ERPManagement.Application.Shared;
using ERPManagement.Application.Shared.Services;

public class MessageLocalizationService : IMessageLocalizationService
{
    private readonly Dictionary<string, Message> _messages;
    private readonly ILanguageService _languageService;

    public MessageLocalizationService(ILanguageService languageService)
    {
        _messages = MessageDataSource.LoadMessages();
        _languageService = languageService;
    }

    public string GetMessage(string key, params object[] args)
    {
        if (!_messages.ContainsKey(key))
            return "Unknown Message Key";

        var message = _languageService.GetLanguageName() switch
        {
            "ar" => _messages[key].MessageArabic,
            "en" => _messages[key].MessageEnglish,
            _ => _messages[key].MessageEnglish
        };

        return string.Format(message, args);
    }
}
