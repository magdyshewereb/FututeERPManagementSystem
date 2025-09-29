namespace ERPManagement.Application.Exceptions
{
	public sealed class DetailListErrorsLines
	{

		public string DtlPropertyName { get; set; }
		public List<int> Lines { get; set; } = new List<int>();
	}
}
