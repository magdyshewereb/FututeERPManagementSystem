namespace ERPManagement.Application.Routing
{
	public static class RouteGenerator
	{
		public static string BaseRoute(string moduleName)
		{
			return $"api/v1/{moduleName.ToLower()}";
		}

		public static string GetAll(string moduleName)
		{
			return $"{BaseRoute(moduleName)}";
		}

		public static string GetById(string moduleName)
		{
			return $"{BaseRoute(moduleName)}/{{id}}";
		}

		public static string Create(string moduleName)
		{
			return $"{BaseRoute(moduleName)}";
		}

		public static string Update(string moduleName)
		{
			return $"{BaseRoute(moduleName)}/{{id}}";
		}

		public static string Delete(string moduleName)
		{
			return $"{BaseRoute(moduleName)}/{{id}}";
		}
	}

	//    // Example for using the RouteGenerator in a controller
	//    [Route(RouteGenerator.BaseRoute("Users"))]
	//    [ApiController]
	//    public class UsersController : ControllerBase
	//    {
	//        [HttpGet]
	//        [Route("")]
	//        public IActionResult GetAll()
	//        {
	//            return Ok();
	//        }

	//        [HttpGet]
	//        [Route("{id}")]
	//        public IActionResult GetById(int id)
	//        {
	//            return Ok();
	//        }
	//    }
	//     API Versioning Setup:الجزء الثاني 
	//     1-   Packageنضيف    
	//         dotnet add package Microsoft.AspNetCore.Mvc.Versioning
	//        2- نضيف Versioning في Program.cs
	//        builder.Services.AddApiVersioning(options =>
	//{
	//        options.DefaultApiVersion = new ApiVersion(1, 0);
	//        options.AssumeDefaultVersionWhenUnspecified = true;
	//        options.ReportApiVersions = true;
	//        options.ApiVersionReader = new UrlSegmentApiVersionReader();
	//    });

	//    3- Controller Example:
	//        [ApiVersion("1.0")]
	//    [Route("api/v{version:apiVersion}/[controller]")]
	//    [ApiController]
	//    public class UsersController : ControllerBase
	//    {
	//        [HttpGet]
	//        public IActionResult GetAll()
	//        {
	//            return Ok();
	//        }
	//    }
	//    ✅ الميزة:
	//✔️ تدعم Multiple Versions بسهولة(v1, v2, v3).

	//✔️ تقدر تكتب نفس Controller بإصدار مختلف:

	//csharp
	//Copy
	//Edit
	//[ApiVersion("2.0")]
	//[Route("api/v{version:apiVersion}/[controller]")]
	//public class UsersV2Controller : ControllerBase
	//    {
	//        // إصدار جديد من الـ API
	//    }


}
