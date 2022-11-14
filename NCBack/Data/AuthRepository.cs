using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NCBack.Dtos.User;
using NCBack.Models;
using NCBack.Services;

namespace NCBack.Data;

public class AuthRepository : IAuthRepository
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment; //Добавляем сервис взаимодействия с файлами в рамках хоста
    private readonly UploadFileService _uploadFileService; // Добавляем сервис для получения файлов из формы
    private readonly PushSms _pushSms;

    public AuthRepository(
        DataContext context,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        IHostEnvironment environment,
        UploadFileService uploadFileService,
        PushSms pushSms)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _environment = environment;
        _uploadFileService = uploadFileService;
        _pushSms = pushSms;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
        .FindFirstValue(ClaimTypes.NameIdentifier));

    public async Task<User> Login(string username, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));
        if (user == null)
        {
            user.Success = false;
            user.Message = "User not found.";
        }
        else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            user.Success = false;
            user.Message = "Wrong password.";
        }
        else
        {
            user.Token = CreateToken(user);
            user.Success = true;
            user.Message = "Done.";
        }

        return user;
    }

    public async Task<User> VerificationCode(int code)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Code.Equals(code));
        if (code != user.Code)
        {
            user.Success = false;
            user.Message = "Code not found.";
        }
        else
        {
            user.Success = true;
            user.Message = "Done.";
            return user;
        }

        return user;
    }

    public async Task<User> Register(
        string city, string phone, string email,
        string username, string fullname, DateTime dateOfBirth,
        IFormFile file, string password)
    {
        User user = new User();

        if (await EmailExists(email))
        {
            user.Success = false;
            user.Message = "Email already exists.";
            return user;
        }

        if (await UserExists(username))
        {
            user.Success = false;
            user.Message = "User already exists.";
            return user;
        }

        await _pushSms.Sms(phone);

        string path = Path.Combine(_environment.ContentRootPath, "wwwroot/images/");
        string photoPath = $"images/{file.FileName}";
        _uploadFileService.Upload(path, file.FileName, file);

        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        user = new User
        {
            City = city,
            PhoneNumber = phone,
            Email = email,
            Code = _pushSms.code,
            Username = username,
            FullName = fullname,
            DateOfBirth = Convert.ToDateTime(dateOfBirth.ToShortDateString()),
            AvatarPath = photoPath,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        user.Id = user.Id;
        user.Message = "Done.";
        return user;
    }

    public async Task<User> SMSNotReceived(string phone, int? id)
    {
        var user = await _context.Users.FindAsync(id);
        await _pushSms.Sms(phone);
        if (user != null)
        {
            user.PhoneNumber = phone;
            user.Code = _pushSms.code;
            user.Message = "Done.";
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        return null;
    }

    public async Task<bool> UserExists(string username)
    {
        if (await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
        {
            return true;
        }

        return false;
    }

    public async Task<bool> EmailExists(string email)
    {
        if (await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
        {
            return true;
        }

        return false;
    }

    public async Task<User> ChangePassword(UserChangePasswordDto request)
    {
        User user = _context.Users.FirstOrDefault(u => u.Id == GetUserId());
        if (user == null)
        {
            user.Success = false;
            user.Message = "User not found.";
        }
        else if (!VerifyPasswordHash(request.CurrentPassword, user.PasswordHash, user.PasswordSalt))
        {
            user.Success = false;
            user.Message = "Wrong password.";
        }
        else
        {
            user.Success = true;
            user.Message = "Done.";
            CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        return user;
    }

    /*public async Task<User> ForgotPassword(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u=> u.Email == email);
        if (user != null)
            {
                string newPsw = PasswordGeneratorService.Generate();
                CreatePasswordHash(newPsw, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                _context.Update(user);
                await EmailService.SendPassword(email, newPsw);
                user.Success = true;
                user.Message = "Done";
                await _context.SaveChangesAsync();
                return user;
            }
            else
            user.Success = false;
            user.Message = "Not found !!!";
        return user;
    }*/

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computeHash.SequenceEqual(passwordHash);
        }
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
            .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}