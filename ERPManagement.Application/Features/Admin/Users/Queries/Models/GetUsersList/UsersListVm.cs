namespace ERPManagement.Application.Features.Admin.Users.Queries.Models.GetUsersList
{
	public class UsersListVm
	{
		public int ID { get; set; }
		public string? NameAr { get; set; }
		public string? NameEn { get; set; }
		public string? Email { get; set; }
		public string? Password { get; set; }
		public string? UserName { get; set; }
		public bool? IsAdmin { get; set; }
	}
}
