using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineShop.HttpApiClient
{
	public class ValidationProblemDetails
	{
		[JsonPropertyName("title")] public string? Title { get; set; }
		[JsonPropertyName("status")] public string? Status { get; set; }
		[JsonPropertyName("errors")] public string? Errors { get; set; }
	}
}
