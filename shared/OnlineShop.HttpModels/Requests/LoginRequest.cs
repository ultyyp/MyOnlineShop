using System.ComponentModel.DataAnnotations;

namespace OnlineShop.HttpModels.Requests
{
	public class LoginRequest
	{
		[Required, EmailAddress]
		public string Email { get; set; }


		[Required]
		[StringLength(30, ErrorMessage = "Password must between 8 and 30 characters.", MinimumLength = 8)]
		public string Password { get; set; }
	}
}