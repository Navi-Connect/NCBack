using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NCBack.Data;
using NCBack.Models;
using NCBack.Models.ModelViews;
using NCBack.Services;

namespace NCBack.Controllers;

[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly DataContext _context;
 
    private readonly IHostEnvironment _environment; //Добавляем сервис взаимодействия с файлами в рамках хоста
    private readonly UploadFileService _uploadFileService; // Добавляем сервис для получения файлов из формы

    public AuthController(IConfiguration configuration, DataContext context, IHostEnvironment environment, UploadFileService uploadFileService)
    {
        _configuration = configuration;
        _context = context;

        _environment = environment;
        _uploadFileService = uploadFileService;
    }

    [HttpPost("register")]
    [RequestSizeLimit(100000000)]
    public async Task<ActionResult> Register([FromForm] UserRegisterView request)
    {
        if (_context.Users.Any(u => u.Username == request.Username))
        {
            return BadRequest("User already exists.");
        }

        string path = Path.Combine(_environment.ContentRootPath, "wwwroot/images/");
        string photoPath = $"images/{request.File.FileName}";
        _uploadFileService.Upload(path, request.File.FileName, request.File);

        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        User user = new User
        {
            City = request.City,
            Username = request.Username,
            FirstName = request.FirstName,
            Lastname = request.Lastname,
            SurName = request.SurName,
            AvatarPath = photoPath,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(UserLoginView request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user?.Username != request.Username)
        {
            return BadRequest("User not found.");
        }

        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Wrong password.");
        }
        
        user.Token = CreateToken(user);
        return Ok(user);
    }


    private string CreateToken(User user)
    {
        /*List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };*/

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac
                .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
