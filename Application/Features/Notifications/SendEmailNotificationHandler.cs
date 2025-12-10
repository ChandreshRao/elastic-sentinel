using ElasticSentinel.Application.Common.Abstractions;
using System.Net.Mail;

namespace ElasticSentinel.Application.Features.Notifications;

/// <summary>
/// Sends email notifications using SMTP.
/// Supports To, CC recipients and HTML email bodies.
/// </summary>
internal sealed class SendEmailNotificationHandler : ISendEmailNotificationHandler
{
    public async Task HandleAsync(
        SendEmailNotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        var emailConnectorDetails = request.EmailConnectorDetails;

        using MailMessage emailMessage = new();
        emailMessage.From = new MailAddress(emailConnectorDetails.FromEmail);

        if (emailConnectorDetails.ToEmails != null)
        {
            foreach (var address in emailConnectorDetails.ToEmails.Split(','))
            {
                emailMessage.To.Add(address);
            }
        }

        if (emailConnectorDetails.CcEmails != null)
        {
            foreach (var address in emailConnectorDetails.CcEmails.Split(','))
            {
                emailMessage.CC.Add(address);
            }
        }

        emailMessage.Subject = emailConnectorDetails.EmailSubject ?? "Application Alert";
        emailMessage.IsBodyHtml = true;
        emailMessage.Body = request.Message;

        using SmtpClient smtpClient = new(emailConnectorDetails.SMTPServer, emailConnectorDetails.SMTPPort);
        await smtpClient.SendMailAsync(emailMessage, cancellationToken);
    }
}
