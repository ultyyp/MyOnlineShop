using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotiSendDotNet;
using NotiSendDotNet.Models.Requests;
using NotiSendDotNet.Models.Responses;
using OnlineShop.Domain.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace OnlineShop.EmailSender.NotiSend
{
    public class NotiSendEmailSender : IEmailSender, IAsyncDisposable
    {
        private readonly NotiSendConfig _notiSendConfig;
        private readonly ILogger<NotiSendEmailSender> _logger;
        private readonly INotiSendClient _client;

        public NotiSendEmailSender(IOptionsSnapshot<NotiSendConfig> optionsSnapshot, ILogger<NotiSendEmailSender> logger)
        {
            ArgumentNullException.ThrowIfNull(optionsSnapshot);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _notiSendConfig = optionsSnapshot.Value;
            _client = INotiSendClient.Create(_notiSendConfig.Token, _notiSendConfig.Host);
        }
        public async ValueTask DisposeAsync()
        {
        }
        public async Task SendEmailAsync(string recepientEmail, string subject, string? body, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(recepientEmail);
            ArgumentNullException.ThrowIfNull(subject);
            ArgumentNullException.ThrowIfNull(body);

            var request = new SendEmailRequest(
                fromEmail: _notiSendConfig.SendFrom,
                fromName: _notiSendConfig.UserName,
                to: recepientEmail,
                subject: subject,
                text: body
            );

            _logger.LogInformation($"NotiSend. Email sent with body: {body}");
            SendEmailResponse response = await _client.SendEmail(request);
            _logger.LogInformation($"NotiSend. Response status code is: {response.Status}");
        }
    }
}