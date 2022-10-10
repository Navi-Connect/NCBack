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
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Приложения Navi Connect",
            "busindavis@yandex.kz"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new BodyBuilder() { HtmlBody = message }.ToMessageBody();
        emailMessage.Prepare(EncodingConstraint.EightBit);
        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.yandex.ru", 25, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync("busindavis@yandex.kz", "rindoman666");
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
    
    public static async Task SendPassword(string email, string password)
    {
        string message = $"<p>Здравствуйте!</p><p>Вы были зарегистрированы в Navi Connect </p>" +
                         $"<p>Используйте одноразовый пароль для входа <b>{password}</b></p>" +
                         $"<p>Войти с ним можно только один раз. Как авторизуетесь, тут же поменяйте пароль. Никому его не говорите!</p>";
        await SendMessageAsync(email, "С уважением Navi Connect", message);
    }
}