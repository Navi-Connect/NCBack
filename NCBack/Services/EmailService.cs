using MailKit.Net.Smtp;
using MimeKit;

using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using MailKit.Security;

namespace NCBack.Services;

public class EmailService
{
    public static async Task SendMessageAsync(string email, string subject, string message)
    {
        
        var apiKey = "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";
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("SupNaviConnect@gmail.com", "Navi Connect");
        var to = new EmailAddress(email, " ");
        var plainTextContent = "С уважением Navi Connect";
        var htmlContent = message;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        await client.SendEmailAsync(msg);
    }
    
    public static async Task SendPassword(string email, string password)
    {
        string message = $"<p>Здравствуйте!</p><p>Вы были зарегистрированы в Navi Connect </p>" +
                         $"<p>Используйте одноразовый пароль для входа <b>{password}</b></p>" +
                         $"<p>Войти с ним можно только один раз. Как авторизуетесь, тут же поменяйте пароль. Никому его не говорите!</p>";
        await SendMessageAsync(email, "С уважением Navi Connect", message);
    }
}