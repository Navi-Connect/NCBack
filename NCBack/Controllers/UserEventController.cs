using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;
using NCBack.Filter;
using NCBack.Helpers;
using NCBack.Models;
using NCBack.NotificationModels;
using NCBack.Services;

namespace NCBack.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserEventController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUriService _uriServiceNews;
    private readonly INotificationService _notificationService;
    private ISession _session => _httpContextAccessor.HttpContext.Session;
    public UserEventController(DataContext context, IHttpContextAccessor httpContextAccessor, IUriService uriServiceNews, INotificationService notificationService)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _uriServiceNews = uriServiceNews;
        _notificationService = notificationService;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
        .FindFirstValue(ClaimTypes.NameIdentifier));

    [Authorize]
    [HttpGet("myOrganizerListEvents")]
    public async Task<IActionResult> MyOrganizerListEvents([FromQuery] ObjectPaginationFilter? filter)
    {
        try
        {
            var list = await (from e in _context.Events
                where e.UserId == GetUserId()
                where e.Status != Status.Accepted
                orderby e
                select new { e.Id, e.AimOfTheMeetingId, e.AimOfTheMeeting, e.MeetingCategoryId, e.MeetingCategory, e.MeatingPlaceId, e.MeatingPlace,
                    e.IWant,e.TimeStart, e.TimeFinish, e.CreateAdd, e.CityId, e.City, e.GenderId, e.Gender,
                    e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                    e.Interests, e.Latitude, e.Longitude, e.UserId, e.User.Gender.GenderName, e.User, e.Status }).Distinct().ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = list
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            pagedData.Reverse();
            var totalRecords = list.Count();
            var pagedReponse = PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
            return Ok(pagedReponse);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("ERROR");
    }

  
    
    
    [Authorize]
    [HttpPost("requestEvent")]
    public async Task<IActionResult> RequestEvent(int eventId)
    {
        var userEvent =
            await _context.UserEvent.FirstOrDefaultAsync(ue => ue.EventId == eventId && ue.UserId == GetUserId());
        
        if (userEvent != null)
        {
            return BadRequest("Вы уже откликнулись на это событя !!!");
        }
        
        try
        {
            var events = _context.Events.FirstOrDefault(e => e.Id == eventId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == events.UserId);
           
            if (events.UserId != GetUserId())
            {
                _context.UserEvent.Add(new UserEvent(GetUserId(), events.Id));
                await _context.SaveChangesAsync();

                var mPlace = _context.MeatingPlace.FirstOrDefault(p => p.Id == events.MeatingPlaceId);
                
                NotificationModel notificationModel = new NotificationModel()
                {
                    UserId = events.UserId,
                    DeviceId = PasswordGeneratorService.OffHesh(events.User.DeviceId),
                    IsAndroiodDevice = true,
                    Title = "К вам поступила заявка",
                    Body = $"на ваше объявление: \n" +
                           $"{mPlace.NameMeatingPlace} от {events.TimeStart.Value.Date.ToString("dd/MM")} с {events.TimeStart.Value.ToString("HH:mm")} по {events.TimeStart.Value.Date.ToString("dd/MM")} до {events.TimeFinish.Value.ToString("HH:mm")}.",
                    DateTime =  DateTime.Now
                };
                
                await _notificationService.SendNotification(notificationModel);
                _context.NotificationModel.Add(new NotificationModel(notificationModel.Id, notificationModel.UserId,notificationModel.IsAndroiodDevice ,notificationModel.Title, notificationModel.Body, notificationModel.DateTime));
                await _context.SaveChangesAsync();
                
                return Ok("Вы успешно отправили заявку !!!");
            }
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("Вы создатель этого обьявления !!!");
    }

    
    
    /*[Authorize]
    [HttpPost("requestEvent")]
    public async Task<IActionResult> RequestEvent(int eventId, [FromQuery] ObjectPaginationFilter? filter)
    {
        try
        {
            var events = _context.Events.FirstOrDefault(e => e.Id == eventId);
            var user =await _context.Users.FirstOrDefaultAsync(u => u.Id == events.UserId);
            DateTime now = DateTime.Now;
            if (events.UserId != GetUserId())
            {
                _context.UserEvent.Add(new UserEvent(GetUserId(), events.Id));
                await _context.SaveChangesAsync();

                var list = (from ev in _context.UserEvent
                    from u in _context.Users
                    from e in _context.Events
                    where ev.UserId == GetUserId()
                    where ev.EventId == e.Id
                    where ev.Event.AimOfTheMeetingId == e.AimOfTheMeeting.Id && ev.Event.MeetingCategoryId == e.MeetingCategory.Id && ev.Event.MeatingPlaceId == e.MeatingPlace.Id
                    where ev.User.Id == ev.UserId
                    where ev.Event.City.Id == e.CityId && ev.Event.Gender.Id == e.GenderId
                    where ev.User.CityId == u.City.Id && ev.User.GenderId == u.Gender.Id
                    where e.Status == Status.Expectations || e.Status == Status.Canceled
                    where e.TimeStart >= now
                    orderby ev
                    select new { ev.Id, ev.User, ev.Event }).Distinct();
                NotificationModel notificationModel = new NotificationModel()
                {
                    UserId = events.UserId,
                    DeviceId = PasswordGeneratorService.OffHesh(events.User.DeviceId),
                    IsAndroiodDevice = true,
                    Title = "К вам поступила заявка",
                    Body = $"На ваще обьявление",
                    DateTime =  DateTime.Now
                };
                await _notificationService.SendNotification(notificationModel);
                _context.NotificationModel.Add(new NotificationModel(notificationModel.Id, notificationModel.UserId,notificationModel.IsAndroiodDevice ,notificationModel.Title, notificationModel.Body, notificationModel.DateTime));
                await _context.SaveChangesAsync();
                var route = Request.Path.Value;
                var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
                var pagedData = list
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                pagedData.Reverse();
                var totalRecords = list.Count();
                var pagedReponse = PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
                return Ok(pagedReponse);
                
                return Ok(list);
            }
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("Users Error !!!");
    }*/
    
    
    [Authorize]
    [HttpGet("myListRequestEventsParticipant")]
    public async Task<IActionResult> MyListRequestEventsParticipant([FromQuery] ObjectPaginationFilter? filter)
    {
        try
        {
            var list = await (from e in _context.UserEvent
                    where e.UserId == GetUserId()
                    select new {e.Id, e.EventId, e.Event.AimOfTheMeetingId, e.Event.AimOfTheMeeting, e.Event.MeetingCategoryId, e.Event.MeetingCategory, e.Event.MeatingPlaceId, e.Event.MeatingPlace,
                        e.Event.IWant,e.Event.TimeStart, e.Event.TimeFinish, e.Event.CreateAdd, e.Event.CityId, e.Event.City, e.Event.GenderId, e.Event.Gender,
                        e.Event.AgeTo, e.Event.AgeFrom, e.Event.CaltulationType, e.Event.CaltulationSum, e.Event.LanguageCommunication,
                        e.Event.Interests, e.Event.Latitude, e.Event.Longitude, e.Event.UserId, e.Event.User.Gender.GenderName, e.Event.User, e.Event.Status}
                ).Distinct().ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = list
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            pagedData.Reverse();
            var totalRecords = list.Count();
            var pagedReponse = PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
            return Ok(pagedReponse);
            
            return Ok(list);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("ERROR !!!");
    }

    [Authorize]
    [HttpGet("listRequestOrganizerEventUsers/{id}")]
    public async Task<IActionResult> ListRequestOrganizerEventUser(int id, [FromQuery] ObjectPaginationFilter? filter)
    {
        try
        {
            var events = _context.Events.FirstOrDefault(e => e.Id == id);
            if (events != null)
            {
                var list = await (from e in _context.UserEvent
                        from c in  _context.CityList
                        from g in  _context.GenderList
                        from a in  _context.AimOfTheMeeting
                        from mc in  _context.MeetingCategory
                        from mp in  _context.MeatingPlace
                        where e.EventId == events.Id
                        where e.Event.UserId == GetUserId()
                        where c.Id  == e.Event.City.Id && g.Id == e.Event.Gender.Id
                       // where e.User.Gender.Id == g.Id && e.User.City.Id == c.Id
                        select new {  e.Id, e.EventId, e.Event.AimOfTheMeetingId, e.Event.AimOfTheMeeting, e.Event.MeetingCategoryId, e.Event.MeetingCategory, e.Event.MeatingPlaceId, e.Event.MeatingPlace,
                            e.Event.IWant,e.Event.TimeStart, e.Event.TimeFinish, e.Event.CreateAdd, e.Event.CityId, e.Event.City, e.Event.GenderId, e.Event.Gender,
                            e.Event.AgeTo, e.Event.AgeFrom, e.Event.CaltulationType, e.Event.CaltulationSum, e.Event.LanguageCommunication,
                            e.Event.Interests, e.Event.Latitude, e.Event.Longitude, e.UserId, e.User.Gender.GenderName,  e.User, e.Event.Status}
                    ).Distinct().ToListAsync();
                var route = Request.Path.Value;
                var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
                var pagedData = list
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                pagedData.Reverse();
                var totalRecords = list.Count();
                var pagedReponse = PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
                return Ok(pagedReponse);
                return Ok(list);
            }

            return BadRequest("ERROR");
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("ERROR");
    }

    [Authorize]
    [HttpPost("acceptUser/{eventId}")]
    public async Task<IActionResult> AcceptUser(int eventId, int userId)
    {
        try
        {
            var events = _context.Events.FirstOrDefault(e => e.Id == eventId);
            var users = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (events.UserId == GetUserId())
            {
                if (events.Id != null)
                {
                    events.UserId = GetUserId();
                    events.Status = Status.Accepted;
                    _context.Events.Update(events);
                    await _context.SaveChangesAsync();

                    if (users.Id != null)
                    {
                        var uE = _context.UserEvent.FirstOrDefault(u => u.UserId == userId && u.EventId == eventId);
                        /*var uEvent = _context.UserEvent.FirstOrDefault(e => e.EventId == eventId);*/
                        //var eUser = _context.UserEvent.FirstOrDefault(u => u.UserId == userId );

                        if (uE != null)
                        {
                            _context.AccedEventUser.Add(new AccedEventUser(uE.UserId, uE.EventId));
                            await _context.SaveChangesAsync();
                        }
                        else
                            BadRequest("Not mathes U and E");

                        /*if (uEvent != null)
                        {
                            /*if (eUser != null)
                            {#1#
                                _context.UserEvent.Remove(uEvent);
                                await _context.SaveChangesAsync();
                            /*}#1#
                        }*/

                        /*sing (var ctx = new UserEvent(userId, eventId))
                        {
                            var x = (from y in _context.UserEvent
                                where  y.UserId == userId
                                where  y.EventId == eventId
                                select y).FirstOrDefault();
                            if(x!=null)
                            {
                                ctx.DeleteObject(x);
                                ctx.SaveChanges();
                            }
                        }*/

                        var delobj =
                            _context.UserEvent.Where(p => p.UserId == userId).ToList();
                        foreach (var v in delobj)
                        {
                            _context.UserEvent.Remove(v);
                            await _context.SaveChangesAsync();
                        }


                        var delobj1 =
                            _context.UserEvent.Where(p => p.EventId == eventId).ToList();
                        foreach (var v in delobj1)
                        {
                            _context.UserEvent.Remove(v);
                            await _context.SaveChangesAsync();
                        }


                        /*
                        if (uE != null)
                        {
                            _context.UserEvent.ToList().RemoveAll(r =>  r.UserId == userId && r.EventId == eventId);
                            await _context.SaveChangesAsync();
                        }
                        */


                        var userEvent = (from ev in _context.AccedEventUser
                            where ev.UserId == userId
                            orderby ev
                            select new { ev.Id, ev.User.Username, ev.User.PhoneNumber }).Distinct();

                        /*var userEvent = (from ev in _context.UserEvent
                            from u in _context.Users
                            from e in _context.Events
                            where ev.UserId == userId
                            where ev.EventId == eventId
                            where ev.User.Id == ev.UserId
                            where e.Status == Status.Accepted
                            orderby ev
                            select new { ev.Id, ev.User, ev.Event }).ToList().Distinct();*/
                        
                        var mPlace = _context.MeatingPlace.FirstOrDefault(p => p.Id == events.MeatingPlaceId);
                        
                        NotificationModel notificationModel = new NotificationModel()
                        {
                            UserId = userId,
                            DeviceId = PasswordGeneratorService.OffHesh(users.DeviceId),
                            IsAndroiodDevice = true,
                            Title = "Поздравляем",
                            Body = $"с предстоящей встречей/событием \n " +
                                   $"{mPlace.NameMeatingPlace} от {events.TimeStart.Value.Date.ToString("dd/MM")} с {events.TimeStart.Value.ToString("HH:mm")} по {events.TimeStart.Value.Date.ToString("dd/MM")} до {events.TimeFinish.Value.ToString("HH:mm")}. \n" +
                                   $"Желаем вам приятной встречи. \n" +
                                   $"Контакты Организатора открыты внутри Профиля \n" +
                                   $"в: “Участник > Одобренные”.." ,
                            DateTime =  DateTime.Now
                        };
                        await _notificationService.SendNotification(notificationModel);
                        _context.NotificationModel.Add(new NotificationModel(notificationModel.Id, notificationModel.UserId,notificationModel.IsAndroiodDevice ,notificationModel.Title, notificationModel.Body, notificationModel.DateTime));
                        await _context.SaveChangesAsync();
                        return Ok(userEvent);
                    }

                    return BadRequest("no user");
                }

                return BadRequest("no Event");
            }

            return BadRequest("ERROR");
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }
        return BadRequest("ERROR");
    }

    [Authorize]
    [HttpPost("canceledUser/{eventId}")]
    public async Task<IActionResult> CanceledUser(int eventId, int userId)
    {
        try
        {
            var events = _context.Events.FirstOrDefault(e => e.Id == eventId);
            var users = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (events.UserId == GetUserId())
            {
                if (events != null)
                {
                    events.UserId = GetUserId();
                    events.Status = Status.Canceled;
                    _context.Events.Update(events);
                    await _context.SaveChangesAsync();

                    if (users != null)
                    {
                        var uE = _context.UserEvent.FirstOrDefault(u => u.UserId == userId && u.EventId == eventId);

                        if (uE != null)
                        {
                            _context.UserEvent.Remove(uE);
                            await _context.SaveChangesAsync();
                        }

                        
                        var userName = _context.Users.FirstOrDefault(u => u.Id == events.UserId);
                        var mPlace = _context.MeatingPlace.FirstOrDefault(p => p.Id == events.MeatingPlaceId);
                        
                        NotificationModel notificationModel = new NotificationModel()
                        {
                            UserId = userId,
                            DeviceId = PasswordGeneratorService.OffHesh(users.DeviceId),
                            IsAndroiodDevice = true,
                            Title = "Ваша заявка отклонена",
                            Body = $"К сожалению {userName.Username}  f  v не отреагировал на \n " + 
                                   $"вашу заявку по объявлению \n" +
                                   $"{mPlace.NameMeatingPlace} от {events.TimeStart.Value.Date.ToString("dd/MM")} с {events.TimeStart.Value.ToString("HH:mm")} по {events.TimeStart.Value.Date.ToString("dd/MM")} до {events.TimeFinish.Value.ToString("HH:mm")}. \n" +
                                   $"У вас есть возможность Создать своё \n" +
                                   $"объявление или Подать заявку другому Connectёру. \n" +
                                   $"К сведению \n" +
                                   $"Возможно у вас не заполен профиль \n" +
                                   $"Максимально заполненный профиль даёт \n" +
                                   $"больше премуществ! Сделайте 'ревизию' себя \n" +
                                   $"Удачи, уважемый Connecter! ",
                            DateTime =  DateTime.Now
                        };
                        
                        await _notificationService.SendNotification(notificationModel);
                        _context.NotificationModel.Add(new NotificationModel(notificationModel.Id, notificationModel.UserId,notificationModel.IsAndroiodDevice ,notificationModel.Title, notificationModel.Body, notificationModel.DateTime));
                        await _context.SaveChangesAsync();
                        
                        return Ok("Успешно отклонено !!!");
                    }
                }
            }

            return BadRequest("ERROR");
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("ERROR");
    }

    /*[Authorize]
    [HttpGet("acceptedEvents")]
    public async Task<IActionResult> AcceptedEvents()
    {
        try
        {
            var userEvent = (from ev in _context.UserEvent 
                from e in _context.Events
                where ev.UserId == GetUserId()
                where ev.EventId == e.Id
                where e.Status == Status.Accepted
                select new { ev.Id, ev.User, ev.Event }).Distinct();
            return Ok(userEvent);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("ERROR");
    }*/

    /*[Authorize]
    [HttpGet("canceledEvents")]
    public async Task<IActionResult> CanceledEvents()
    {
        try
        {
            var userEvent = (from ev in _context.UserEvent
                from e in _context.Events
                where ev.UserId == GetUserId()
                where ev.EventId == e.Id
                where e.Status == Status.Canceled
                select new { ev.Id, ev.User, ev.Event }).Distinct();
            return Ok(userEvent);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("ERROR");
    }*/
    
    
    [Authorize]
    [HttpGet("acceptOrganizerEventUsers")]
    public async Task<IActionResult> AcceptOrganizerEventUsers([FromQuery] ObjectPaginationFilter? filter)
    {
        try
        {
            var acceptUserEvent = await (from aeu in _context.AccedEventUser
                where aeu.Event.UserId == GetUserId()
                where aeu.UserId == aeu.User.Id
                select new { aeu.Id, aeu.UserId, aeu.User.Username, aeu.User.PhoneNumber, aeu.EventId, aeu.Event.AimOfTheMeetingId, aeu.Event.AimOfTheMeeting, aeu.Event.MeetingCategoryId, aeu.Event.MeetingCategory, aeu.Event.MeatingPlaceId, aeu.Event.MeatingPlace,
                    aeu.Event.IWant,aeu.Event.TimeStart, aeu.Event.TimeFinish, aeu.Event.CreateAdd, aeu.Event.CityId, aeu.Event.City, aeu.Event.GenderId, aeu.Event.Gender,
                    aeu.Event.AgeTo, aeu.Event.AgeFrom, aeu.Event.CaltulationType, aeu.Event.CaltulationSum, aeu.Event.LanguageCommunication,
                    aeu.Event.Interests, aeu.Event.Latitude, aeu.Event.Longitude, aeu.Event.User.Gender.GenderName, aeu.Event.User, aeu.Event.Status
                }).Distinct().ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = acceptUserEvent
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            pagedData.Reverse();
            var totalRecords = acceptUserEvent.Count();
            var pagedReponse = PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
            return Ok(pagedReponse);
            return Ok(acceptUserEvent);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("ERROR");
    }
    
    [Authorize]
    [HttpGet("acceptParticipantEventUsers")]
    public async Task<IActionResult> AcceptParticipantEventUsers([FromQuery] ObjectPaginationFilter? filter)
    {
        try
        {
            var acceptUserEvent = await (from aeu in _context.AccedEventUser
                where aeu.UserId == GetUserId()
                where aeu.UserId == aeu.User.Id
                select new { aeu.Id, aeu.EventId, aeu.Event.AimOfTheMeetingId, aeu.Event.AimOfTheMeeting, aeu.Event.MeetingCategoryId, aeu.Event.MeetingCategory, aeu.Event.MeatingPlaceId, aeu.Event.MeatingPlace,
                    aeu.Event.IWant,aeu.Event.TimeStart, aeu.Event.TimeFinish, aeu.Event.CreateAdd, aeu.Event.CityId, aeu.Event.City, aeu.Event.GenderId, aeu.Event.Gender,
                    aeu.Event.AgeTo, aeu.Event.AgeFrom, aeu.Event.CaltulationType, aeu.Event.CaltulationSum, aeu.Event.LanguageCommunication,
                    aeu.Event.Interests, aeu.Event.Latitude, aeu.Event.Longitude, aeu.Event.UserId, aeu.Event.User.Gender.GenderName, aeu.Event.User,  aeu.Event.Status
                }).Distinct().ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = acceptUserEvent
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            pagedData.Reverse();
            var totalRecords = acceptUserEvent.Count();
            var pagedReponse = PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
            return Ok(pagedReponse);
            return Ok(acceptUserEvent);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("ERROR");
    }
    
}