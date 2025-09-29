using ERPManagement.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ERPManagement.Application.Shared.Helpers
{
	public static class HttpResponseHelper
	{
		public static IActionResult ToHttpResponse<T>(this BaseResponse<T> response)
		{
			return response.StatusCode switch
			{
				HttpStatusCode.OK => new OkObjectResult(response),
				HttpStatusCode.Created => new CreatedResult(string.Empty, response),
				HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(response),
				HttpStatusCode.BadRequest => new BadRequestObjectResult(response),
				HttpStatusCode.NotFound => new NotFoundObjectResult(response),
				HttpStatusCode.Accepted => new AcceptedResult(string.Empty, response),
				HttpStatusCode.UnprocessableEntity => new UnprocessableEntityObjectResult(response),
				_ => new BadRequestObjectResult(response),
			};
		}

	}
}
