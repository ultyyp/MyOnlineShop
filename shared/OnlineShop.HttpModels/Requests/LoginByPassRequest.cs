using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.HttpModels.Requests
{
    public class LoginByPassRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Password Needs To Have 8 Characters", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
