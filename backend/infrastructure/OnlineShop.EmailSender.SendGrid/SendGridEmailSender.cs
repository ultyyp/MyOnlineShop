using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.EmailSender.SendGrid
{
    /// <summary>
    /// Implementation of the email sender using SendGrid service.
    /// </summary>
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient _client;
        private readonly SendGridConfig _config;

        /// <summary>
        /// Initializes a new instance of the SendGridEmailSender class.
        /// </summary>
        /// <param name="optionsSnapshot">The config gotten from secrets.json.</param>
        public SendGridEmailSender(IOptionsSnapshot<SendGridConfig> optionsSnapshot)
        {
            ArgumentNullException.ThrowIfNull(optionsSnapshot);
            _config = optionsSnapshot.Value;
            _client = new SendGridClient(_config.ApiKey);
        }


        /// <summary>
        /// Sends an email asynchronously using SendGrid.
        /// </summary>
        /// <param name="recipient">The email address of the recipient.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <returns>A Task representing the asynchronous operation with the response.</returns>
        public async Task SendEmailAsync(string recipient, string subject, string? body, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(recipient);
            ArgumentNullException.ThrowIfNull(subject);
            ArgumentNullException.ThrowIfNull(body);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_config.FromEmail, "OnlineShopPoC"),
                Subject = subject,
                PlainTextContent = body,
                HtmlContent = $"<strong>{body}</strong>"
            };

            msg.AddTo(new EmailAddress(recipient, "To user"));
            var response = await _client.SendEmailAsync(msg).ConfigureAwait(false);
            if(!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Email Could Not Be Sent! Error Code:{response.StatusCode}, Error Body:{response.Body}");
            }
        }

        
    }


}
