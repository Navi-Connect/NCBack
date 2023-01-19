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
                orderby e
                select new { e.Id, e.AimOfTheMeetingId, e.AimOfTheMeeting, e.MeetingCategoryId, e.MeetingCategory, e.MeatingPlaceId, e.MeatingPlace,
                    e.IWant,e.TimeStart, e.TimeFinish, e.CreateAdd, e.CityId, e.City, e.GenderId, e.Gender,
                    e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                    e.Interests, e.Latitude, e.Longitude, e.UserId, e.User, e.Status }).Distinct().ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = list
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = list.Count();
            var pagedReponse = PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
            return Ok(pagedReponse);
            return Ok(list);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("ERROR");
    }

    [Authorize]
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
                    where ev.User.StatusRequest == StatusUserRequest.Expectation
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
                    DeviceId = _session.GetString("DeviceId"),
                    IsAndroiodDevice = true,
                    Title = "К вам поступила заявка",
                    Body = $"На ваще обьявление"
                };
                await _notificationService.SendNotification(notificationModel);
                var route = Request.Path.Value;
                var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
                var pagedData = list
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
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
    }

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
                        e.Event.Interests, e.Event.Latitude, e.Event.Longitude, e.Event.UserId, e.Event.User, e.Event.Status}
                ).Distinct().ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = list
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
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
                        where c.Id  == e.Event.CityId && g.Id == e.Event.GenderId
                        select new {  e.Id, e.EventId, e.Event.AimOfTheMeetingId, e.Event.AimOfTheMeeting, e.Event.MeetingCategoryId, e.Event.MeetingCategory, e.Event.MeatingPlaceId, e.Event.MeatingPlace,
                            e.Event.IWant,e.Event.TimeStart, e.Event.TimeFinish, e.Event.CreateAdd, e.Event.CityId, e.Event.City, e.Event.GenderId, e.Event.Gender,
                            e.Event.AgeTo, e.Event.AgeFrom, e.Event.CaltulationType, e.Event.CaltulationSum, e.Event.LanguageCommunication,
                            e.Event.Interests, e.Event.Latitude, e.Event.Longitude, e.UserId, e.User, e.Event.Status}
                    ).Distinct().ToListAsync();
                var route = Request.Path.Value;
                var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
                var pagedData = list
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
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
                    events.User.StatusRequest = StatusUserRequest.Empty;
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
                            where ev.User.StatusRequest == StatusUserRequest.Empty
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
                        NotificationModel notificationModel = new NotificationModel()
                        {
                            UserId = userId,
                            DeviceId = _session.GetString("DeviceId"),
                            IsAndroiodDevice = true,
                            Title = "Поздравляем с предстоящей встречей/событием !",
                            Body = $"Вашу заявку “РЕСТОРАН от 21/12 с 18:00 - до 21:00” подтвердили. Желаем вам отлично провести время." +
                                   $"\n Контакты открыты в вашем профиле." 
                        };
                        await _notificationService.SendNotification(notificationModel);

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
                    events.User.StatusRequest = StatusUserRequest.Empty;
                    events.Status = Status.Canceled;
                    _context.Events.Update(events);
                    await _context.SaveChangesAsync();

                    if (users != null)
                    {
                        var uE = _context.UserEvent.FirstOrDefault(u => u.UserId == userId && u.EventId == eventId);

                        if (uE != null)
                            _context.UserEvent.Remove(uE);
                        await _context.SaveChangesAsync();
                        NotificationModel notificationModel = new NotificationModel()
                        {
                            UserId = userId,
                            DeviceId = _session.GetString("DeviceId"),
                            IsAndroiodDevice = true,
                            Title = "Уважаемый Connectёр!",
                            Body = $"{events.User.Username} выбрал другого Connectёра. У вас есть возможность !!!"
                        };
                        await _notificationService.SendNotification(notificationModel);
                        return Ok("Canceled Done !!!");
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
                select new { aeu.Id, aeu.UserId, aeu.User, aeu.EventId, aeu.Event.AimOfTheMeetingId, aeu.Event.AimOfTheMeeting, aeu.Event.MeetingCategoryId, aeu.Event.MeetingCategory, aeu.Event.MeatingPlaceId, aeu.Event.MeatingPlace,
                    aeu.Event.IWant,aeu.Event.TimeStart, aeu.Event.TimeFinish, aeu.Event.CreateAdd, aeu.Event.CityId, aeu.Event.City, aeu.Event.GenderId, aeu.Event.Gender,
                    aeu.Event.AgeTo, aeu.Event.AgeFrom, aeu.Event.CaltulationType, aeu.Event.CaltulationSum, aeu.Event.LanguageCommunication,
                    aeu.Event.Interests, aeu.Event.Latitude, aeu.Event.Longitude,  aeu.Event.Status
                }).Distinct().ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = acceptUserEvent
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
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
                    aeu.Event.Interests, aeu.Event.Latitude, aeu.Event.Longitude, aeu.Event.UserId, aeu.Event.User,  aeu.Event.Status
                }).Distinct().ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = acceptUserEvent
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
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