namespace ERPManagement.Application.Routing
{
	public static class ApiRoutes
	{
		public const string Root = "api";
		public const string Version = "v1";
		public const string Base = Root + "/" + Version;
		public static class Main
		{
			public const string List = Base + "/Index";
			public const string Create = Base + "/AddData";
			public const string Edit = Base + "/UpdateData";
			public const string Delete = Base + "/DeleteData";
		}
		public static class Authentication
		{
			public const string Login = Base + "/auth/login";
			public const string Register = Base + "/auth/register";
			public const string RefreshToken = Base + "/auth/refresh";
			public const string ConfirmEmail = Base + "/auth/confirm-email";
			public const string ResetPassword = Base + "/auth/reset-password";
		}

		public static class Users
		{
			public const string GetAll = Base + "/users";
			public const string GetById = Base + "/users/{id}";
			public const string Create = Base + "/users";
			public const string Update = Base + "/users/{id}";
			public const string Delete = Base + "/users/{id}";
		}

		public static class Tickets
		{
			public const string GetAll = Base + "/tickets";
			public const string GetById = Base + "/tickets/{id}";
			public const string Create = Base + "/tickets";
			public const string Update = Base + "/tickets/{id}";
			public const string Delete = Base + "/tickets/{id}";
		}

		public static class Accounts
		{
			public const string GetAll = Base + "/accounts";
			public const string GetById = Base + "/accounts/{id}";
			public const string Create = Base + "/accounts";
			public const string Update = Base + "/accounts/{id}";
			public const string Delete = Base + "/accounts/{id}";
		}
	}
}
// :Controller مثال استخدامه في الـ

//[Route(ApiRoutes.Users.GetAll)]
//[ApiController]
//public class UsersController : ControllerBase
//{

//}
//[Route("api/v1/[controller]")]
//[ApiController]
//public class UsersController : ControllerBase
//{
//    [HttpGet]
//    [Route("")]
//    public IActionResult GetUsers()
//    {
//        return Ok();
//    }
//}
