

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
    private readonly INotificationService _notificationService;
    private readonly TokenSetting _tokenSetting;
    private ISession _session => _httpContextAccessor.HttpContext.Session;

    public AuthRepository(
        DataContext context,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        IHostEnvironment environment,
        UploadFileService uploadFileService,
        PushSms pushSms,
        INotificationService notificationService,
        IOptions<TokenSetting> tokenSetting)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _environment = environment;
        _uploadFileService = uploadFileService;
        _pushSms = pushSms;
        _notificationService = notificationService;
        _tokenSetting = tokenSetting.Value;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
        .FindFirstValue(ClaimTypes.NameIdentifier));

    public async Task<User> Login(string username, string password, string? deviceId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));
        if (user.Username == null)
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
            if (deviceId != null)
            {
                user.DeviceId = PasswordGeneratorService.ToHesh(deviceId);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            
            await _context.CityList.FirstAsync(c => c.Id == user.CityId);
            if (user.GenderId != null)
                await _context.GenderList.FirstAsync(c => c.Id == user.GenderId);

            var accessToken = CreateToken(user);
            var resfreshToken = CreateRefreshToken();

            await InsertRefrashToke(user.Id, resfreshToken);

            user.AccessToken = accessToken;
            user.RefreshToken = resfreshToken;

            return user;

            /*new TokenDto
            {
                /*UserId = user.Id,#1#
                /*CityId = user.CityId,
                City = user.City,
                Email = user.Email,
                Username = user.Username,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                AvatarPath = user.AvatarPath,
                GenderId = user.GenderId,
                Gender = user.Gender,#1#
                AccessToken = accessToken,
                RefreshToken = resfreshToken,
                /*DeviceId = user.DeviceId,
                Success = user.Success,
                Message = user.Message#1#
                
            };*/

        }
        return null;
    }

    public async Task<User> VerificationCode(int? id, int code)
    {
        var intermediateUser = await _context.IntermediateUser
            .FirstOrDefaultAsync(u => u.Code.Equals(code) && u.Id == id);
        User user = new User();
        
        if (code != intermediateUser.Code)
        {
            intermediateUser.Success = false;
            intermediateUser.Message = "Code not found.";
        }
        else
        {
            
            user = new User()
            {
                CityId = intermediateUser.CityId,
                City = await _context.CityList.FirstAsync(c => c.Id == intermediateUser.CityId),
                Email = intermediateUser.Email,
                Username = intermediateUser.Username,
                Code = intermediateUser.Code,
                PhoneNumber = intermediateUser.PhoneNumber,
                FullName = intermediateUser.FullName,
                DateOfBirth = intermediateUser.DateOfBirth,
                GenderId = intermediateUser.GenderId,
                Gender = await _context.GenderList.FirstAsync(c => c.Id == intermediateUser.GenderId),
                AvatarPath = intermediateUser.AvatarPath,
                PasswordHash = intermediateUser.PasswordHash,
                PasswordSalt = intermediateUser.PasswordSalt,
                Message = "Very good Done !!!",
                Success = true,
            };
            
            if (await UserExists(user.Username))
            {
                user.Success = false;
                user.Message = "User already exists.";
                return user;
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            intermediateUser.Success = true;
            intermediateUser.Message = "Done.";
            
            var delUsr =
                _context.IntermediateUser.Where(u => u.Id == id).ToList();
            foreach (var usr in delUsr)
            {
                _context.IntermediateUser.Remove(usr);
                await _context.SaveChangesAsync();
            }
            
            return user;
        }
        
        return user;
    }

    public async Task<IntermediateUser> Register(
        int cityId, string email,
        string username, string fullname, DateTime dateOfBirth, int genderId,
        IFormFile file, string password)
    {
        IntermediateUser user = new IntermediateUser();

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

        //await _pushSms.Sms(phone);

        string path = Path.Combine(_environment.ContentRootPath, "wwwroot/images/");
        string photoPath = $"images/{file.FileName}";
        _uploadFileService.Upload(path, file.FileName, file);

        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        IntermediateUser intermediateUser = new IntermediateUser
        {
            CityId = cityId,
            City = await _context.CityList.FirstAsync(c => c.Id == cityId),
            Email = email,
            Code = 0,
            Username = username,
            FullName = fullname,
            DateOfBirth = Convert.ToDateTime(dateOfBirth.ToShortDateString()),
            GenderId = genderId,
            Gender = await _context.GenderList.FirstAsync(c => c.Id == genderId),
            AvatarPath = photoPath,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        _context.IntermediateUser.Add(intermediateUser);
        await _context.SaveChangesAsync();
        user.Id = user.Id;
        user.Success = true;
        user.Message = "User in intermediate table !!!";
        return intermediateUser;
    }

    public async Task<IntermediateUser> SMSNotReceived(int? id, string phone)
    {
        var intermediateUser = await _context.IntermediateUser.FindAsync(id);
        
        if (await PhoneExists(phone))
        {
            intermediateUser.Success = false;
            intermediateUser.Message = "Phone already exists.";
            return intermediateUser;
        }
        
        await _pushSms.Sms(phone);

        if (intermediateUser != null)
        {
            await _context.CityList.FirstAsync(c => c.Id == intermediateUser.CityId);
            await _context.GenderList.FirstAsync(c => c.Id == intermediateUser.GenderId);
            intermediateUser.PhoneNumber = phone;
            intermediateUser.Code = _pushSms.code;
            intermediateUser.Message = "Done.";
            _context.IntermediateUser.Update(intermediateUser);
            await _context.SaveChangesAsync();


            /*_context.Entry(intermediateUser).State = EntityState.Deleted;
            _context.IntermediateUser.Remove(intermediateUser);
            await _context.SaveChangesAsync();*/

            return intermediateUser;
        }

        return null;
    }

    public async Task<User> RenewTokens(RefreshTokenDto refreshToken)
    {
        var userRefreshtoken = await _context
            .RefreshToken.Where(_ => _.Token == refreshToken.Token &&
                                     _.ExpirationDate >= DateTime.Now).FirstOrDefaultAsync();
        
        if (userRefreshtoken == null)
        {
            return null;
        }

        var user = await _context.Users.Where(_ => _.Id == userRefreshtoken.UserId).FirstOrDefaultAsync();

        var newJwtToken = CreateToken(user);
        var newRefreshToken = CreateRefreshToken();
        
        userRefreshtoken.Token = newRefreshToken;
        userRefreshtoken.ExpirationDate = DateTime.Now.AddDays(7);
        await _context.SaveChangesAsync();

        user.AccessToken = newJwtToken;
        user.RefreshToken = newRefreshToken;

        return user;
        /*new TokenDto
        {
            /*UserId = user.Id,#1#
            /*CityId = user.CityId,
            City = user.City,
            Email = user.Email,
            Username = user.Username,
            FullName = user.FullName,
            DateOfBirth = user.DateOfBirth,
            AvatarPath = user.AvatarPath,
            GenderId = user.GenderId,
            Gender = user.Gender,#1#
            AccessToken = newJwtToken,
            RefreshToken = newRefreshToken,
            /*DeviceId = user.DeviceId,
            Success = user.Success,
            Message = user.Message#1#
        };*/
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
    
    public async Task<bool> PhoneExists(string phone)
    {
        if (await _context.Users.AnyAsync(u => u.PhoneNumber.ToLower() == phone.ToLower()))
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
    

    /*private string CreateToken(User user)
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
    }*/

    private string CreateToken(User user)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_tokenSetting.SecretKey));

        var credentials = new SigningCredentials(
            symmetricSecurityKey,
            SecurityAlgorithms.HmacSha256);

        var userClaims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var jwtToken = new JwtSecurityToken(
            issuer: _tokenSetting.Issuer,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials,
            claims: userClaims,
            audience: _tokenSetting.Audience
        );

        string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return token;
    }

    private  string CreateRefreshToken()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(tokenBytes);

        var tokenIsInUse =  
            _context.RefreshToken.Any(_ => _.Token == refreshToken);

        if (tokenIsInUse)
        {
            return CreateRefreshToken();
        }

        return refreshToken;
    }

    private async Task InsertRefrashToke(int userId, string refreshToken)
    {
        var newRefreshToken = new RefreshToken
        {
            UserId = userId,
            Token = refreshToken,
            ExpirationDate = DateTime.Now.AddDays(7)
        };

        _context.RefreshToken.Add(newRefreshToken);
        await _context.SaveChangesAsync();
    }

}