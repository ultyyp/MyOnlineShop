using System.ComponentModel.DataAnnotations;

namespace OnlineShop.EmailSender.SendGrid;

public class SendGridConfig
{
    [Required]
    public string ApiKey { get; set; }
    [Required, EmailAddress]
    public string FromEmail { get; set; }
}