using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;
using NCBack.Dtos.User;
using NCBack.Services;

namespace NCBack.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHostEnvironment _environment; //Добавляем сервис взаимодействия с файлами в рамках хоста
    private readonly UploadFileService _uploadFileService; // Добавляем сервис для получения файлов из формы
    private readonly PushSms _pushSms;

    public UserController(DataContext context, IHttpContextAccessor httpContextAccessor, IHostEnvironment environment, UploadFileService uploadFileService, PushSms pushSms)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _environment = environment;
        _uploadFileService = uploadFileService;
        _pushSms = pushSms;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
        .FindFirstValue(ClaimTypes.NameIdentifier));

    [HttpGet]
    public async Task<ActionResult> GetUser()
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == GetUserId());
        return Ok(user);
    }

    [HttpGet("getUserById")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        return Ok(user);
    }

    
    
    [HttpPost("editPhotoProfile")]
    public async Task<IActionResult> EditingProfile([FromForm] UserEditPhotoDto model)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == GetUserId()); 
        if (user != null)
        {
            if (user.AvatarPath != null)
            {
                string path = Path.Combine(_environment.ContentRootPath, "wwwroot/images/");
                string photoPath = $"images/{model.File.FileName}";
                _uploadFileService.Upload(path, model.File.FileName, model.File);
                user.AvatarPath = photoPath;
            }
        }
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }
    
    [HttpPost("editProfile")]
    public async Task<IActionResult> EditingProfile([FromForm] UserEditDto model)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == GetUserId()); 
        if (user != null)
        {
            if (user.AvatarPath != null)
            {
                user.CredoAboutMyself = model.CredoAboutMyself;
                user.LanguageOfCommunication = model.LanguageOfCommunication;
                user.Nationality = model.Nationality;
                user.Gender = model.Gender; ;
                user.MaritalStatus = model.MaritalStatus;
                user.IWantToLearn = model.IWantToLearn;
                user.PreferredPlaces = model.PreferredPlaces;
                user.Interests = model.Interests; 
                user.GetAcquaintedWith = model.GetAcquaintedWith;
                user.MeetFor = model.MeetFor;
                user.From = model.From;
                user.To = model.To;
                user.Profession = model.Profession;
            }
        }
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPost("editUsername")]
    public async Task<IActionResult> EditUsername(UserEditUserNameDto model)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == GetUserId());

        if (await _context.Users.AnyAsync(u => u.Username.ToLower() == model.Username.ToLower()))
        {
            user.Success = false;
            user.Message = "User already exists.";
            return BadRequest("Error User already exists !!!");
        }

        if (model.Username != null)
        {
            user.Username = model.Username;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        return BadRequest("Error !!!");
    }
    
    [HttpPost("editPersonalInformation")]
    public async Task<IActionResult> EditingPersonalInformation(UserEditPersonalInformationDto model)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == GetUserId());

        if (user != null)
        {
            user.City = model.City;
            user.FullName = model.FullName;
        }
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPost("EditingContactInformation")]
    public async Task<IActionResult> EditingContactInformation(UserEditContactDto model)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == GetUserId());
        await _pushSms.Sms(model.Phone);
        
        if (user != null)
        {
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            user.Code = _pushSms.code;
        }
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }
    
    [HttpGet("users")]
    public async Task<IActionResult> Users()
    {
        var user = _context.Users.ToList();
        return Ok(user);
    }
    
    [HttpPost("deleteUser")]
    public async Task<IActionResult> DeleteUser()
    {
        var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == GetUserId());
        
        if (user is null)
            return NotFound();

        _context.Entry(user).State = EntityState.Deleted;
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return Ok("Success delete user !!!");
    }
    
}