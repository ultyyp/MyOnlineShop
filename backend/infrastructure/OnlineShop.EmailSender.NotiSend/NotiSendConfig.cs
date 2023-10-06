using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.EmailSender.NotiSend
{
    public class NotiSendConfig
    {
        [Required, RegularExpression(@"[^@\s]+\.[^@\s]+\.[^@\s]+$")] public string Host { get; set; }
        [Required] public string UserName { get; set; }
        [Required] public string Token { get; set; }
        [EmailAddress, Required] public string SendFrom { get; set; }
    }
}
