using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace TangyWeb_API.Helper;

public class EmailSender : IEmailSender
{
    public string SendGridSecret { get; set; }

    public EmailSender(IConfiguration _config)
    {
        SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey");
    }

    public async Task SendEmailWithMailkitAsync(string email, string subject, string htmlMessage)
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
            throw; // Never use throw(ex), because the stack trace will be lost
        }
    }

    //This Method does not work for me, since I have no real domain and also no SebndGrid account.
    public async Task SendEmailWithSendGridAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            var client = new SendGridClient(SendGridSecret);
            var from = new EmailAddress("hello@ultimateaps.nl", "Tangy");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            await client.SendEmailAsync(msg);
        }
        catch (Exception)
        {
            throw; // Never use throw(ex), because the stack trace will be lost

        }
    }
}
