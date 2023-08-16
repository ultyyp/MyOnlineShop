using System.ComponentModel.DataAnnotations;

namespace OnlineShop.HttpModels.Requests
{
    //DTO - Data transfer object
    public class RegisterRequest
    {
        [Required]
        [StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Password must between 8 and 30 characters.", MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirming your password is required.")]
        [Compare(nameof(Password))]
        public string ConfirmedPassword { get; set; }

    }
}
