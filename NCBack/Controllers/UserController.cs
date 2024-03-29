﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;
using NCBack.Dtos.User;
using NCBack.Filter;
using NCBack.Helpers;
using NCBack.Models;
using NCBack.NotificationModels;
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
    private readonly INotificationService _notificationService;
    private readonly IUriService _uriServiceNews;
    private ISession _session => _httpContextAccessor.HttpContext.Session;

    public UserController(DataContext context, IHttpContextAccessor httpContextAccessor, IHostEnvironment environment,
        UploadFileService uploadFileService, PushSms pushSms, INotificationService notificationService,
        IUriService uriServiceNews)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _environment = environment;
        _uploadFileService = uploadFileService;
        _pushSms = pushSms;
        _notificationService = notificationService;
        _uriServiceNews = uriServiceNews;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
        .FindFirstValue(ClaimTypes.NameIdentifier));

    [HttpGet("getNotificationById")]
    public async Task<ActionResult> GetNotificationById(int id)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            var notification = await _context.NotificationModel
                .FirstOrDefaultAsync(n => user != null && n.Id == id && n.UserId == user.Id);
            if (notification != null)
            {
                notification.Status = true;
                _context.NotificationModel.Update(notification);
                await _context.SaveChangesAsync();
                return Ok(notification);
            }

            return BadRequest("Erorr not fount notification !!!");
        }
        catch (ApplicationException e)
        {
            return BadRequest("Erorr not fount notification !!!");
        }
    }

    [HttpGet("getNotificationsList")]
    public async Task<ActionResult> GetNotificationsList([FromQuery] ObjectPaginationFilter? filter)
    {
        try
        {
            var notification = await _context.NotificationModel.Where(n => n.UserId == GetUserId())
                .Distinct().OrderByDescending(n => n.Id).ToListAsync();
          //  PaginationHelper.ReversEventList(notification);
            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = notification
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = notification.Count();
            var pagedReponse =
                PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
            return Ok(pagedReponse);
        }
        catch (ApplicationException e)
        {
            return BadRequest("Error 400!!!");
        }
    }

    [HttpDelete("deleteNotification/{id}")]
    public async Task<ActionResult<List<News>>> DeleteNotification(int id)
    {
        try
        {
            var notification = await _context.NotificationModel.FindAsync(id);
            if (notification != null)
            {
                _context.NotificationModel.Remove(notification);
                await _context.SaveChangesAsync();
                return Ok(notification);
            }
        }
        catch (ApplicationException e)
        {
            return BadRequest("Error 400!!!");
        }

        return BadRequest("System errors !!!");
    }

    [HttpGet("getUser")]
    public async Task<ActionResult> GetUser()
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            user.City = _context.CityList.FirstOrDefault(c => c.Id == user.CityId);
            user.Gender = _context.GenderList.FirstOrDefault(g => g.Id == user.GenderId);
            
            user.Age = DateTime.Today.Year - user.DateOfBirth.Year;
            if (DateTime.Today < user.DateOfBirth.AddYears(user.Age))
            {
                user.Age--;
            }
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            
            return Ok(user);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }
    }

    [HttpGet("getUserById")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            user.City = _context.CityList.FirstOrDefault(c => c.Id == user.CityId);
            user.Gender = _context.GenderList.FirstOrDefault(g => g.Id == user.GenderId);
            
            user.Age = DateTime.Today.Year - user.DateOfBirth.Year;
            if (DateTime.Today < user.DateOfBirth.AddYears(user.Age))
            {
                user.Age--;
            }
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
        catch (ApplicationException e)
        {
            return BadRequest("Error 400!!!");
        }

        BadRequest("Error");
    }


    
    [HttpPost("editPhotoProfile")]
    public async Task<IActionResult> EditingProfile([FromForm] UserEditPhotoDto model)
    {
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == GetUserId());
            if (user != null)
            {
                if (user.AvatarPath != null)
                {
                    //string path = Path.Combine(_environment.ContentRootPath, "wwwroot/images/");
                    string photoPath = $"sampleimage/{model.File.FileName}";
                    await _uploadFileService.Upload(model.File);
                    user.AvatarPath = photoPath;
                    user.City = _context.CityList.FirstOrDefault(c => c.Id == user.CityId);
                    user.Gender = _context.GenderList.FirstOrDefault(g => g.Id == user.GenderId);
                }
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
        catch (ApplicationException e)
        {
            return BadRequest("Error 400!!!");
        }

        BadRequest("Error");
    }

    [HttpPost("editProfile")]
    public async Task<IActionResult> EditingProfile([FromForm] UserEditDto model)
    {
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == GetUserId());
            if (user != null)
            {
                if (user.AvatarPath != null)
                {
                    user.CityId = model.CityId;
                    user.City = _context.CityList.FirstOrDefault(c => c.Id == model.CityId);
                    user.FullName = model.FullName;
                    user.CredoAboutMyself = model.CredoAboutMyself;
                    user.LanguageOfCommunication = model.LanguageOfCommunication;
                    user.Nationality = model.Nationality;
                    user.GenderId = model.GenderId;
                    user.Gender = _context.GenderList.FirstOrDefault(g => g.Id == model.GenderId);
                    user.MaritalStatus = model.MaritalStatus;
                    user.Сhildren = model.Сhildren;
                    user.IWantToLearn = model.IWantToLearn;
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
        catch (ApplicationException e)
        {
            return BadRequest("Error 400!!!");
        }

        BadRequest("Error");
    }

    [HttpPost("editUsername")]
    public async Task<IActionResult> EditUsername(UserEditUserNameDto model)
    {
        try
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
                user.City = _context.CityList.FirstOrDefault(c => c.Id == user.CityId);
                user.Gender = _context.GenderList.FirstOrDefault(g => g.Id == user.GenderId);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return Ok(user);
            }

            return BadRequest("Error !!!");
        }
        catch (ApplicationException e)
        {
            return BadRequest("Error 400!!!");
        }

        BadRequest("Error");
    }

    /*[HttpPost("editPersonalInformation")]
    public async Task<IActionResult> EditingPersonalInformation(UserEditPersonalInformationDto model)
    {
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == GetUserId());

            if (user != null)
            {
                user.CityId = model.CityId;
                user.FullName = model.FullName;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        BadRequest("Error");
    }*/

    [HttpPost("editingContactEmail")]
    public async Task<IActionResult> EditingContactEmail(UserEditContactEmailDto model)
    {
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == GetUserId());

            if (user != null)
            {
                if (await _context.Users.AnyAsync(u => u.Email.ToLower() == model.Email.ToLower()))
                {
                    user.Success = false;
                    user.Message = "Email already exists.";
                    return Ok(user);
                }

                user.Email = model.Email;
                user.City = _context.CityList.FirstOrDefault(c => c.Id == user.CityId);
                user.Gender = _context.GenderList.FirstOrDefault(g => g.Id == user.GenderId);

                NotificationModel notificationModel = new NotificationModel()
                {
                    UserId = GetUserId(),
                    DeviceId = PasswordGeneratorService.OffHesh(user.DeviceId),
                    IsAndroiodDevice = true,
                    Title = "Вы меняете свою электронную почту",
                    Body = "Уважаемый пользователь! \n" +
                           "Вы сменили ваш адрес электронной почты \n" +
                           "на новый.",
                    DateTime = DateTime.Now,
                    Status = false
                };
                await _notificationService.SendNotification(notificationModel);
                _context.NotificationModel.Add(new NotificationModel(notificationModel.Id, notificationModel.UserId,
                    notificationModel.IsAndroiodDevice, notificationModel.Title, notificationModel.Body,
                    notificationModel.DateTime, notificationModel.Status));
                await _context.SaveChangesAsync();
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
        catch (ApplicationException e)
        {
            return BadRequest("Error 400!!!");
        }

        BadRequest("Error");
    }

    [HttpPost("editingContactPhone/{Id}")]
    public async Task<IActionResult> EditingContactPhone(int? Id, UserEditContactPhoneDto model)
    {
        try
        {
            await _pushSms.Sms(model.Phone);

            if (model.Phone != null)
            {
                PhoneEditing phoneEditing = new PhoneEditing()
                {
                    Code = _pushSms.code,
                    PhoneNumber = model.Phone,
                };
                _context.PhoneEditing.Add(phoneEditing);
                await _context.SaveChangesAsync();
                return Ok(phoneEditing);
            }

            return BadRequest("Error");
        }
        catch (ApplicationException e)
        {
            return BadRequest("Error 400!!!");
        }
    }

    [HttpPost("verificationCodeEditing/{id}")]
    public async Task<IActionResult> VerificationCodeEditing(int? id, int code)
    {
        try
        {
            var phoneEditing = await _context.PhoneEditing
                .FirstOrDefaultAsync(u => u.Code.Equals(code) && u.Id == id);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            if (code != phoneEditing.Code)
            {
                phoneEditing.Success = false;
                phoneEditing.Message = "Code not found.";
            }
            else
            {
                user.Code = phoneEditing.Code;
                user.PhoneNumber = phoneEditing.PhoneNumber;
                user.Message = "Very good Done !!!";
                user.Success = true;
                user.City = _context.CityList.FirstOrDefault(c => c.Id == user.CityId);
                user.Gender = _context.GenderList.FirstOrDefault(g => g.Id == user.GenderId);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                NotificationModel notificationModel = new NotificationModel()
                {
                    UserId = GetUserId(),
                    DeviceId = PasswordGeneratorService.OffHesh(user.DeviceId),
                    IsAndroiodDevice = true,
                    Title = "Вы успешно сменили номер телефона",
                    Body = "Поздравляем со сменой номера телефона. \n" +
                           "Этот номер будет отображаться другим \n" +
                           "пользователям в объявлениях при Connect.",
                    DateTime = DateTime.Now,
                    Status = false
                };
                await _notificationService.SendNotification(notificationModel);
                _context.NotificationModel.Add(new NotificationModel(notificationModel.Id, notificationModel.UserId,
                    notificationModel.IsAndroiodDevice, notificationModel.Title, notificationModel.Body,
                    notificationModel.DateTime, notificationModel.Status));
                await _context.SaveChangesAsync();
                phoneEditing.Success = true;
                phoneEditing.Message = "Done.";
                return Ok(user);
            }

            return BadRequest("Error");
        }
        catch (ApplicationException e)
        {
            return BadRequest("Error 400!!!");
        }
    }


    [HttpPost("editingContactPhoneSendAgain/{Id}")]
    public async Task<IActionResult> EditingContactPhoneSendAgain(int? Id, UserEditContactPhoneDto model)
    {
        try
        {
            await _pushSms.Sms(model.Phone);

            if (model.Phone != null)
            {
                PhoneEditing phoneEditing = new PhoneEditing()
                {
                    Id = Id,
                    Code = _pushSms.code,
                    PhoneNumber = model.Phone,
                };
                _context.PhoneEditing.Update(phoneEditing);
                await _context.SaveChangesAsync();
                return Ok(phoneEditing);
            }

            return BadRequest("Error");
        }
        catch (ApplicationException e)
        {
            return BadRequest("Error 400!!!");
        }
    }

    [HttpGet("users")]
    public async Task<IActionResult> Users()
    {
        try
        {
            var user = await _context.Users.ToListAsync();
            return Ok(user);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        BadRequest("Error");
    }

    [HttpDelete("deleteUser")]
    public async Task<IActionResult> DeleteUser()
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == GetUserId());
            var events = await _context.Events.FirstOrDefaultAsync(e => e.UserId == GetUserId());
            if (user is null && events is null)
            {
                return NotFound();
            }

            if (events != null)
            {
                _context.Entry(events).State = EntityState.Deleted;
                _context.Events.Remove(events);
                await _context.SaveChangesAsync();
            }

            if (user != null)
            {
                _context.Entry(user).State = EntityState.Deleted;
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok("Success delete user and your events !!!");
            }

            return BadRequest("Error");
        }
        catch (ApplicationException e)
        {
            return BadRequest("Error 400!!!");
        }
    }

    [Authorize]
    [HttpGet("userGetByIdEvents/{id}")]
    public async Task<IActionResult> UserGetByIdEvents(int id, [FromQuery] ObjectPaginationFilter? filter)
    {
        try
        {
            DateTime now = DateTime.Now;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            user.City = _context.CityList.FirstOrDefault(c => c.Id == user.CityId);
            user.Gender = _context.GenderList.FirstOrDefault(g => g.Id == user.GenderId);
            
            var lists = await
                (from e in _context.Events
                    where e.Status == Status.Expectations || e.Status == Status.Canceled
                    where e.TimeStart >= now
                    where e.UserId == user.Id
                    select new 
                    {
                        e.Id, e.AimOfTheMeetingId, e.AimOfTheMeeting, e.MeetingCategoryId, e.MeetingCategory,
                        e.MeatingPlaceId, e.MeatingPlace,
                        e.IWant, e.TimeStart, e.TimeFinish, e.CreateAdd, e.CityId, EVCityName = (e.City.CityName),
                        e.GenderId, EVGenderName = (e.Gender.GenderName),
                        e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                        e.Interests, e.Latitude, e.Longitude, e.Status,
                        e.UserId, USGenderName = (e.User.Gender.GenderName), USCityName = (e.User.City.CityName), e.User
                    }).Distinct().OrderByDescending(e => e.Id).ToListAsync();
            
            if (lists != null)
            {
                var route = Request.Path.Value;
                var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
                var pagedData = lists
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = lists.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
                return Ok(pagedReponse);
            }
            return BadRequest("У коннектера нет обьявлений !!!");
            
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }
    }
    
}