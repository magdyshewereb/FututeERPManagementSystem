namespace ERPManagement.Application.Shared
{
	public class Message
	{
		public string MessageArabic { get; init; }
		public string MessageEnglish { get; init; }

		public Message(string messageArabic, string messageEnglish)
		{
			MessageArabic = messageArabic ?? throw new ArgumentNullException(nameof(messageArabic));
			MessageEnglish = messageEnglish ?? throw new ArgumentNullException(nameof(messageEnglish));
		}
	}

}
