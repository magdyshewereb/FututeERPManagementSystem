namespace ERPManagement.Application.Responses
{
	public class BaseResponseHandler

	{

		public BaseResponse<T> Deleted<T>(string Message = null)
		{
			return new BaseResponse<T>()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Success = true,
				Message = Message
			};
		}
		public BaseResponse<T> Success<T>(T entity, object Meta = null)
		{
			return new BaseResponse<T>()
			{
				Data = entity,
				StatusCode = System.Net.HttpStatusCode.OK,
				Success = true,
				Message = "ok",
				Meta = Meta
			};
		}
		public BaseResponse<T> Unauthorized<T>(string Message = null)
		{
			return new BaseResponse<T>()
			{
				StatusCode = System.Net.HttpStatusCode.Unauthorized,
				Success = true,
				Message = Message == null ? "unauthraized" : Message
			};
		}
		public BaseResponse<T> BadRequest<T>(string Message = null)
		{
			return new BaseResponse<T>()
			{
				StatusCode = System.Net.HttpStatusCode.BadRequest,
				Success = false,
				Message = Message == null ? "Bad" : Message
			};
		}

		public BaseResponse<T> UnprocessableEntity<T>(string Message = null)
		{
			return new BaseResponse<T>()
			{
				StatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
				Success = false,
				Message = Message == null ? "Error" : Message
			};
		}


		public BaseResponse<T> NotFound<T>(string message = null)
		{
			return new BaseResponse<T>()
			{
				StatusCode = System.Net.HttpStatusCode.NotFound,
				Success = false,
				Message = message == null ? "Not Found" : message
			};
		}

		public BaseResponse<T> Created<T>(T entity, object Meta = null)
		{
			return new BaseResponse<T>()
			{
				Data = entity,
				StatusCode = System.Net.HttpStatusCode.Created,
				Success = true,
				Message = "Created",
				Meta = Meta
			};
		}
	}


}
