using OnlineShop.HttpModels.Responses;
using System.Net;

namespace OnlineShop.HttpApiClient
{
	[Serializable]
	public class MyShopApiException : Exception
	{
		public ErrorResponse? Error { get; }

        public HttpStatusCode? StatusCode { get; }

        public ValidationProblemDetails? Details { get; }

        public MyShopApiException(HttpStatusCode statusCode, ValidationProblemDetails details) : base(details.Title)
		{
			StatusCode = statusCode;
			Details = details;
		}

		public MyShopApiException(ErrorResponse error) : base(error.Message)
		{
			Error = error;
		}

		public MyShopApiException(string? message) : base(message)
		{
		}

		public MyShopApiException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

	}
}