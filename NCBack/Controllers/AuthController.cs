using Microsoft.AspNetCore.Authorization;
using NCBack.Data;
using NCBack.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Models;
using NCBack.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NCBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        
        private readonly IAuthRepository _authRepo;
        private readonly DataContext _context;
        private static ISendGridClient _sendGridClient;
        private static IConfiguration _configuration;

        public AuthController(IAuthRepository authRepo, DataContext context, ISendGridClient sendGridClient,
            IConfiguration configuration)
        {
            _authRepo = authRepo;
            _context = context;
            _sendGridClient = sendGridClient;
            _configuration = configuration;
          
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromForm] UserRegisterDto request)
        {
            try
            {
                var response = await _authRepo.Register(
                    request.CityId,
                    request.Email,
                    request.Username,
                    request.Fullname,
                    Convert.ToDateTime(request.DateOfBirth.ToShortDateString()),
                    request.GenderId,
                    request.File,
                    request.Password);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest("Что то пошло не так при регистраци !!!");
            }
        }

        [HttpPost("verificationCode/{Id}")]
        public async Task<ActionResult<User>> VerificationCode(int Id, UserCodeDto request)
        {
            try
            {
                var response = await _authRepo.VerificationCode(
                    Id,
                    request.VerificationCode
                );
                if (response.Success != true)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest("Не правильный код подтверждения !!!");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserLoginDto request)
        {
            try
            {
                var response = await _authRepo.Login(request.Username, request.Password, request.DeviceId);
                if (response == null)
                {
                    return BadRequest("Не правильный логин или пароль !!!");
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest("Не правильный логин или пароль !!!");
            }
        }

        [HttpPost]
        [Route("renew-token")]
        public async Task<ActionResult> RenewTokens(RefreshTokenDto refreshToken)
        {
            try
            {
                var tokens = await _authRepo.RenewTokens(refreshToken);
                if (tokens == null)
                {
                    return BadRequest("Invalide Refresh Token");
                }
                return Ok(tokens);
            }
            catch (Exception e)
            {
                return BadRequest("Invalide Refresh Token !!!! ");
            }
        }

        [HttpPost("smsNotReceived/{id}")]
        public async Task<ActionResult<User>> SMSNotReceived(int? id, string phone)
        {
            try
            {
                var response = await _authRepo.SMSNotReceived(id, phone);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest("Вы вели не правильный номер телефона !!!");
            }
        }

        [HttpPost("changePassword")]
        [Authorize]
        public async Task<ActionResult<User>> ChangePassword(UserChangePasswordDto request)
        {
            try
            {
                var response = await _authRepo.ChangePassword(request);
                if (response.Success != true)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest("При смена пороля вы сделалий ошибку !!!");
            }
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user != null)
                {
                    string newPsw = PasswordGeneratorService.Generate();
                    CreatePasswordHash(newPsw, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    _context.Update(user);
                    await SendPassword(request.Email, newPsw);
                    user.Success = true;
                    user.Message = "Done";
                    await _context.SaveChangesAsync();
                    return Ok(user);
                }
                else
                    user.Success = false;

                user.Message = "Not found !!!";
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest("Вы вели не правильную почту !!!");
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static async Task SendMessageAsync(string email, string subject, string message)
        {
            var fromEmail = _configuration.GetSection("SendGrindEmailSettings")
                .GetValue<string>("FromEmail");
            var fromName = _configuration.GetSection("SendGrindEmailSettings")
                .GetValue<string>("FromName");
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                HtmlContent = message
            };

            msg.AddTo(email);

            await _sendGridClient.SendEmailAsync(msg);
        }

        public static async Task SendPassword(string email, string password)
        {
            string message = $"<p>Здравствуйте!</p><p>Вы были зарегистрированы в Navi Connect </p>" +
                             $"<p>Используйте одноразовый пароль для входа <b>{password}</b></p>" +
                             $"<p>Войти с ним можно только один раз. Как авторизуетесь, тут же поменяйте пароль. Никому его не говорите!</p>";
            await SendMessageAsync(email, "С уважением Navi Connect", message);
        }
    }
}