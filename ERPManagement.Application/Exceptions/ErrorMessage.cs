namespace ERPManagement.Application.Exceptions
{
	public static class ErrorMessage
	{
		public static void AddErrorMsg(this Dictionary<string, string> errorsDictionary, string errormsg)
		{
			errorsDictionary.Add((errorsDictionary.Count + 1).ToString(), errormsg);
		}

	}
}
