namespace ERPManagement.Application.Exceptions
{
	public sealed class ValidationException : AppException
	{
		public Dictionary<string, string> ErrorsDictionary { get; }
		public DetailListErrorsLines DetailsErrorsLines { get; }

		public ValidationException(
			Dictionary<string, string> errorsDictionary,
			DetailListErrorsLines? DetailListErrors = null)
			: base("Validation Failure", "One or more validation errors occurred")
		{
			ErrorsDictionary = errorsDictionary ?? new Dictionary<string, string>();
			DetailsErrorsLines = DetailListErrors ?? new DetailListErrorsLines();
		}
	}
}
