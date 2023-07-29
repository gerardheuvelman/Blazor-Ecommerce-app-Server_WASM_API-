using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace TangyWeb_API.Helper;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        //This Method no longer works with Gmail, due to an internal Google Policy change. More info can be found at:
        //https://support.google.com/accounts/answer/6010255

        try
        {
            //prepare email
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse("hello@ultimateapps.nl"));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            // send email
            using var emailClient = new SmtpClient;
            emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            emailClient.Authenticate("me@gmail.com", "1234567");
            emailClient.SendAsync(emailToSend);
            emailClient.Disconnect(true);

        }
        catch (Exception)
        {
            throw; // Do not use throw(ex), because the stack trace will be lost
        }
    }
}
