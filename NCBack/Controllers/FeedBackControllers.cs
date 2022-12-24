using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NCBack.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBackController : Controller
    {
        private readonly DataContext _context;
        private readonly  ISendGridClient _sendGridClient;
        private readonly  IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FeedBackController(DataContext context, ISendGridClient sendGridClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _sendGridClient = sendGridClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpPost("FeedBackPost")]
        public async Task<ActionResult> Execute(string text)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == GetUserId());
                
                var fromEmail = _configuration.GetSection("SendGrindEmailSettings")
                    .GetValue<string>("FromEmail");
                var fromName = _configuration.GetSection("SendGrindEmailSettings")
                    .GetValue<string>("FromName");
                
                var to = new EmailAddress("@gmail.com", "Ilyas");
                var subject = "Sending with SendGrid is Fun";
              
                var htmlContent
                    = $"<h3>Информация о пльзователея:</h3> Никнейм:{user.Username} ФИ:{user.FullName} \n " +
                      $" <h3><b>Номер пользователя:</b><b>+{user.PhoneNumber} </h3></b> " +
                      $"\n <h3>Почта пользователя: <b>{user.Email}</br></b></br></h3> \n " +
                      $"<h3>Описание проблемы:</h3> <strong>{text}</strong>" + 
                      $"С Уважением {user.FullName}";
                
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(fromEmail, fromName),
                    Subject = subject,
                    HtmlContent = htmlContent
                };
                msg.AddTo(to);
                await _sendGridClient.SendEmailAsync(msg);
                return Ok(msg);
            }
            catch
            {
                return BadRequest("error");
            }
            
        }
    }
}