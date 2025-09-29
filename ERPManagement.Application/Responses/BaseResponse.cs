using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using System.Net;

namespace ERPManagement.Application.Responses
{
	public class BaseResponse<T> : IMessage
	{
		public bool Success { get; set; }
		public string? Message { get; set; } = string.Empty;
		public List<string>? ValidationErrors { get; set; }
		public HttpStatusCode StatusCode { get; set; }
		public object Meta { get; set; } = string.Empty;
		public List<string> Errors { get; set; } = [];
		public int Id { get; set; }
		public T Data { get; set; }

		public BaseResponse()
		{
			Success = true;
		}
		public BaseResponse(string message)
		{
			Success = true;
			Message = message;
		}

		public BaseResponse(string message, bool success)
		{
			Success = success;
			Message = message;
		}
		public BaseResponse(T data, string? message = null)
		{
			Success = true;
			Message = message;
			Data = data;
		}
	}
}
