using Microsoft.AspNetCore.Authorization;
using NCBack.Data;
using NCBack.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using NCBack.Models;

namespace NCBack.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromForm] UserRegisterDto request)
        {
            var response = await _authRepo.Register(
                request.City,
                request.Region,
                request.Username,
                request.FirstName,
                request.Lastname,
                request.SurName,
                request.DateOfBirth,
                request.File,
                request.Password);
            
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserLoginDto request)
        {
            var response = await _authRepo.Login(request.Username, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        
        [HttpPost("changePassword")]
        [Authorize]
        public async Task<ActionResult<User>> ChangePassword(UserChangePasswordDto request)
        {
            var response = await _authRepo.ChangePassword(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}

/*
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;
using NCBack.Models;
using NCBack.Models.ModelViews;
using NCBack.Services;

namespace NCBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IHostEnvironment _environment; //Добавляем сервис взаимодействия с файлами в рамках хоста
        private readonly UploadFileService _uploadFileService; // Добавляем сервис для получения файлов из формы

        public AuthController(DataContext context, IConfiguration configuration, IUserService userService, IHostEnvironment environment, UploadFileService uploadFileService)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
            _environment = environment;
            _uploadFileService = uploadFileService;
        }

        [HttpGet, Authorize]
        public ActionResult<User> GetMe()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromForm] UserRegisterView request)
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
            
            /*
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return Ok(user);#1#
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserLoginView request)
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

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => true);
            
            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if(user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            
            return refreshToken;
        }

        private async void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => true);
            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        private string CreateToken(User user)
        {
            /*List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };#1#

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
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
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
}


/*
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
        };#2#

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
#1#
*/
