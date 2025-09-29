namespace ERPManagement.Application.Features.Admin.UserRoles.Query.Models
{
	public class RolesBusinessObjectsVmQueryResult
	{
		public int Id { get; set; }
		public int BusinessObjectId { get; set; }
		public required string NameAr { get; set; }
		public required string NameEn { get; set; }
		public bool? AddData { get; set; }
		public bool? UpdateData { get; set; }
		public bool? DeleteData { get; set; }

	}
}
