using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.HttpModels.Requests
{
    public class LoginByCodeRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public Guid CodeId { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
