using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;
using NCBack.Dtos.Event;
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

    public UserEventController(DataContext context, IHttpContextAccessor httpContextAccessor,
        IUriService uriServiceNews, INotificationService notificationService)
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
            DateTime now = DateTime.Now;

            var list = await (from e in _context.Events
                where e.UserId == GetUserId()
                where e.Status != Status.Accepted
                where e.TimeStart >= now
                orderby e
                select new
                {
                    e.Id, e.AimOfTheMeetingId, e.AimOfTheMeeting, e.MeetingCategoryId, e.MeetingCategory,
                    e.MeatingPlaceId, e.MeatingPlace,
                    e.IWant, e.TimeStart, e.TimeFinish, e.CreateAdd, e.CityId, EVCityName = (e.City.CityName),
                    e.GenderId, EVGenderName = (e.Gender.GenderName),
                    e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                    e.Interests, e.Latitude, e.Longitude, e.UserId, USGenderName = (e.User.Gender.GenderName),
                    USCityName = (e.User.City.CityName), e.User, e.Status
                }).Distinct().OrderByDescending(e => e.Id).ToListAsync();

            //PaginationHelper.ReversEventList(list);

            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = list
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = list.Count();
            var pagedReponse =
                PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
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

       // var acceptUser = _context.AccedEventUser.Where(a => a.UserId == GetUserId()).ToList();

        if (userEvent != null)
        {
            return BadRequest("Вы уже откликнулись на это событя !!!");
        }

        
        var acceptUser = _context.AccedEventUser.Where(aeu => aeu.UserId == GetUserId()).ToList();
        
        var eventsTime = _context.Events.FirstOrDefault(e => e.Id == eventId);
        
        /*
        foreach (var time in ceckTime)
        {
            if (eventsTime != null &&
                ceckTime != null && 
                (time.DataTimeStartEvent >= eventsTime.TimeStart  
            time.DataTimeFinishEvent == eventsTime.TimeFinish))
            { 
                return BadRequest( "Вы уже Откликнулисть на Это время");
            }
            */

            foreach (var timeAeu in acceptUser)
            {
                /*( time1Start <= time2Finish && time1Finish >= time2Start) || (time2Start <= time1Finish && time2Finish >= time1Start)
                
                var time1Start = timeAeu.TimeStartEventUser;
                var time1Finish = timeAeu.TimeFinishEventUser;

                var time2Start = eventsTime.TimeStart;
                var time2Finish = eventsTime.TimeFinish;*/
                
                if (eventsTime != null && 
                    (timeAeu.TimeStartEventUser <= eventsTime.TimeFinish && timeAeu.TimeFinishEventUser >= eventsTime.TimeStart) || 
                    (eventsTime.TimeStart  <= timeAeu.TimeFinishEventUser && eventsTime.TimeFinish >= timeAeu.TimeStartEventUser))
                {
                    return BadRequest( "У вас уже запланировано это время !!!");
                }
            }
            
            
            
        /*}*/
        
        /*foreach (var acceptEventUser in acceptUser)
        {
            //var start  = Convert.ToDateTime(acceptEventUser.TimeStartEventUser.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
            //var finish  = Convert.ToDateTime(acceptEventUser.TimeFinishEventUser.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));

            // var now1 = Convert.ToDateTime(now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));


            if (now >= Convert.ToDateTime(
                    acceptEventUser.TimeStartEventUser.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")) && now <=
                Convert.ToDateTime(acceptEventUser.TimeFinishEventUser.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")))
            {
                return BadRequest("Это время уже занято !!!");
            }
            else
            {
            }
        }*/
        
        
        
        try
        {
            var events = _context.Events.FirstOrDefault(e => e.Id == eventId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == events.UserId);

            if (events.UserId != GetUserId())
            {
                
                // получаем текущее время
                DateTime now = DateTime.Now;

                var userEventList = await _context.UserEvent.Where(u=> u.UserId == GetUserId()).ToListAsync();
                    
                foreach (var ue in userEventList)
                {
                    // проверяем, превышает ли разница заданное время для удаления
                    if (ue.TimeResult > now)
                    {
                        return BadRequest($"Вы не можете отправить заявку до {ue.TimeResult.ToString("HH:mm")} или пока вас не примут или отклонят !!!");
                    }
                }

               
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
                               $"{mPlace.NameMeatingPlace} от {events.TimeStart.Value.Date.ToString("dd/MM")} с {events.TimeStart.Value.ToString("HH:mm")} по {events.TimeStart.Value.Date.ToString("dd/MM")} до {events.TimeFinish.Value.ToString("HH:mm")}. \n" +
                               $"Чтобы посмотреть аватар Connectёра, \n" +
                               $"зайдите в свой “Профиль”: \n" +
                               $"Организатор > Объявления (Откройте это \n" +
                               $" объявление) > Заявки на встречу \n" +
                               $"Рассмотрите Connectёра, так как \n" +
                               $"заявка автоматически удалится \n" +
                               $" через 2 часа",
                        DateTime = DateTime.Now,
                        Status = false
                    };

                    await _notificationService.SendNotification(notificationModel);
                    _context.NotificationModel.Add(new NotificationModel(notificationModel.Id,
                        notificationModel.UserId,
                        notificationModel.IsAndroiodDevice, notificationModel.Title, notificationModel.Body,
                        notificationModel.DateTime, notificationModel.Status));


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
            // получаем текущее время
            DateTime now = DateTime.Now;

            var userEvent = await _context.UserEvent.ToListAsync();

            foreach (var ue in userEvent)
            {
                // проверяем, превышает ли разница заданное время для удаления
                if (ue.TimeResult < now)
                {
                    // удаляем пользователя из базы данных
                    _context.UserEvent.Remove(ue);
                }
            }
            
            // сохраняем изменения в базе данных
            await _context.SaveChangesAsync();
            
            var list = await (from e in _context.UserEvent
                    where e.UserId == GetUserId()
                    select new
                    {
                        e.Id, TimeResult = (e.TimeResult.ToString("HH:mm")) , e.EventId, e.Event.AimOfTheMeetingId, e.Event.AimOfTheMeeting, e.Event.MeetingCategoryId,
                        e.Event.MeetingCategory, e.Event.MeatingPlaceId, e.Event.MeatingPlace,
                        e.Event.IWant, e.Event.TimeStart, e.Event.TimeFinish, e.Event.CreateAdd, e.Event.CityId,
                        EVCityName = (e.Event.City.CityName), e.Event.GenderId,
                        EVGenderName = (e.Event.Gender.GenderName),
                        e.Event.AgeTo, e.Event.AgeFrom, e.Event.CaltulationType, e.Event.CaltulationSum,
                        e.Event.LanguageCommunication,
                        e.Event.Interests, e.Event.Latitude, e.Event.Longitude, e.Event.UserId,
                        USGenderName = (e.Event.User.Gender.GenderName), USCityName = (e.Event.User.City.CityName),
                        e.Event.User, e.Event.Status
                    }
                ).Distinct().OrderByDescending(e => e.Id).ToListAsync();

            //PaginationHelper.ReversEventList(list);

            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = list
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = list.Count();
            var pagedReponse =
                PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
            return Ok(pagedReponse);
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
                // получаем текущее время
                DateTime now = DateTime.Now;

                var userEvent = await _context.UserEvent.ToListAsync();

                foreach (var ue in userEvent)
                {
                    // проверяем, превышает ли разница заданное время для удаления
                    if (ue.TimeResult < now)
                    {
                        // удаляем пользователя из базы данных
                        _context.UserEvent.Remove(ue);
                    }
                }

                // сохраняем изменения в базе данных
                await _context.SaveChangesAsync();

                var list = await (from e in _context.UserEvent
                        from a in _context.AimOfTheMeeting
                        from mc in _context.MeetingCategory
                        from mp in _context.MeatingPlace
                        where e.EventId == events.Id
                        where e.Event.UserId == GetUserId()
                        select new
                        {
                            e.Id, e.EventId, ToTime = (e.CreateAt.ToString("HH:mm")),
                            FromTime = (e.TimeResult.ToString("HH:mm")), e.Event.AimOfTheMeetingId,
                            e.Event.AimOfTheMeeting, e.Event.MeetingCategoryId, e.Event.MeetingCategory,
                            e.Event.MeatingPlaceId, e.Event.MeatingPlace,
                            e.Event.IWant, e.Event.TimeStart, e.Event.TimeFinish, e.Event.CreateAdd, e.Event.CityId,
                            EVCityName = (e.Event.City.CityName), e.Event.GenderId,
                            EVGenderName = (e.Event.Gender.GenderName),
                            e.Event.AgeTo, e.Event.AgeFrom, e.Event.CaltulationType, e.Event.CaltulationSum,
                            e.Event.LanguageCommunication,
                            e.Event.Interests, e.Event.Latitude, e.Event.Longitude, e.UserId,
                            USGenderName = (e.User.Gender.GenderName), USCityName = (e.User.City.CityName), e.User,
                            e.Event.Status
                        }
                    ).Distinct().OrderByDescending(e => e.Id).ToListAsync();

               // PaginationHelper.ReversEventList(list);

                var route = Request.Path.Value;
                var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
                var pagedData = list
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = list.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews,
                        route);
                return Ok(pagedReponse);
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
                            _context.AccedEventUser.Add(new AccedEventUser(uE.UserId, uE.EventId, events.TimeStart.GetValueOrDefault(), events.TimeFinish.GetValueOrDefault()));
                            await _context.SaveChangesAsync();
                            var accept =
                                _context.AccedEventUser.FirstOrDefault(u => u.UserId == userId && u.EventId == eventId);
                            accept!.AccedNotifications = Models.AccedNotification.NotVerified;
                            _context.UserEvent.Remove(uE);
                            await _context.SaveChangesAsync();
                        }
                        else
                            BadRequest("ERROR");

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


                        /*var delobj =
                            _context.UserEvent.Where(p => p.UserId == userId).ToList();
                        foreach (var v in delobj)
                        {
                            _context.UserEvent.Remove(v);
                            await _context.SaveChangesAsync();
                        }*/


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
                        var usersPhone = _context.Users.FirstOrDefault(u => u.Id == GetUserId());
                        NotificationModel notificationModel = new NotificationModel()
                        {
                            UserId = userId,
                            DeviceId = PasswordGeneratorService.OffHesh(users.DeviceId),
                            IsAndroiodDevice = true,
                            Title = "Поздравляем",
                            Body = $"C предстоящей встречей/событием \n " +
                                   $"{mPlace.NameMeatingPlace} от {events.TimeStart.Value.Date.ToString("dd/MM")} с {events.TimeStart.Value.ToString("HH:mm")} по {events.TimeStart.Value.Date.ToString("dd/MM")} до {events.TimeFinish.Value.ToString("HH:mm")}. \n " +
                                   $"Желаем вам приятной встречи. \n " +
                                   $"Контакты  Организатора открыты внутри Профиля \n " +
                                   $"в: “Участник > Одобренные”.. \n " +
                                   $"+{usersPhone.PhoneNumber}",
                            DateTime = DateTime.Now,
                            Status = false
                        };
                        await _notificationService.SendNotification(notificationModel);
                        _context.NotificationModel.Add(new NotificationModel(notificationModel.Id,
                            notificationModel.UserId, notificationModel.IsAndroiodDevice, notificationModel.Title,
                            notificationModel.Body, notificationModel.DateTime, notificationModel.Status));
                        await _context.SaveChangesAsync();
                        return Ok(userEvent);
                    }

                    return BadRequest("ERROR");
                }

                return BadRequest("ERROR");
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
                            Body = $"К сожалению {userName.Username} не отреагировал на \n " +
                                   $"вашу заявку по объявлению \n " +
                                   $"{mPlace.NameMeatingPlace} от {events.TimeStart.Value.Date.ToString("dd/MM")} с {events.TimeStart.Value.ToString("HH:mm")} по {events.TimeStart.Value.Date.ToString("dd/MM")} до {events.TimeFinish.Value.ToString("HH:mm")}. \n " +
                                   $"У вас есть возможность Создать своё \n " +
                                   $"объявление или Подать заявку другому Connectёру. \n " +
                                   $"К сведению \n " +
                                   $"Возможно у вас не заполен профиль \n " +
                                   $"Максимально заполненный профиль даёт \n " +
                                   $"больше премуществ! Сделайте 'ревизию' себя \n " +
                                   $"Удачи, уважемый Connecteр! ",
                            DateTime = DateTime.Now,
                            Status = false
                        };

                        await _notificationService.SendNotification(notificationModel);
                        _context.NotificationModel.Add(new NotificationModel(notificationModel.Id,
                            notificationModel.UserId, notificationModel.IsAndroiodDevice, notificationModel.Title,
                            notificationModel.Body, notificationModel.DateTime, notificationModel.Status));
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
    
    [Authorize]
    [HttpGet("acceptOrganizerEventUsers")]
    public async Task<IActionResult> AcceptOrganizerEventUsers([FromQuery] ObjectPaginationFilter? filter)
    {
        try
        {
            var acceptUserEvent = await (from aeu in _context.AccedEventUser
                where aeu.Event.UserId == GetUserId()
                where aeu.UserId == aeu.User.Id
                select new
                {
                    aeu.Id, aeu.UserId, aeu.User.Username, aeu.User.PhoneNumber, aeu.EventId,
                    aeu.Event.AimOfTheMeetingId, aeu.Event.AimOfTheMeeting, aeu.Event.MeetingCategoryId,
                    aeu.Event.MeetingCategory, aeu.Event.MeatingPlaceId, aeu.Event.MeatingPlace,
                    aeu.Event.IWant, aeu.Event.TimeStart, aeu.Event.TimeFinish, aeu.Event.CreateAdd, aeu.Event.CityId,
                    EVCityName = (aeu.Event.City.CityName), aeu.Event.GenderId,
                    EVGenderName = (aeu.Event.Gender.GenderName),
                    aeu.Event.AgeTo, aeu.Event.AgeFrom, aeu.Event.CaltulationType, aeu.Event.CaltulationSum,
                    aeu.Event.LanguageCommunication,
                    aeu.Event.Interests, aeu.Event.Latitude, aeu.Event.Longitude,
                    USGenderName = (aeu.Event.User.Gender.GenderName), USCityName = (aeu.Event.User.City.CityName),
                    aeu.Event.User, aeu.Event.Status ,  aeu.AccedNotifications
                }).Distinct().OrderByDescending(e => e.Id).ToListAsync();

            //PaginationHelper.ReversEventList(acceptUserEvent);

            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = acceptUserEvent
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = acceptUserEvent.Count();
            var pagedReponse =
                PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
            return Ok(pagedReponse);
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
                select new
                {
                    aeu.Id, aeu.EventId, aeu.Event.AimOfTheMeetingId, aeu.Event.AimOfTheMeeting,
                    aeu.Event.MeetingCategoryId, aeu.Event.MeetingCategory, aeu.Event.MeatingPlaceId,
                    aeu.Event.MeatingPlace,
                    aeu.Event.IWant, aeu.Event.TimeStart, aeu.Event.TimeFinish, aeu.Event.CreateAdd, aeu.Event.CityId,
                    EVCityName = (aeu.Event.City.CityName), aeu.Event.GenderId,
                    EVGenderName = (aeu.Event.Gender.GenderName),
                    aeu.Event.AgeTo, aeu.Event.AgeFrom, aeu.Event.CaltulationType, aeu.Event.CaltulationSum,
                    aeu.Event.LanguageCommunication,
                    aeu.Event.Interests, aeu.Event.Latitude, aeu.Event.Longitude, aeu.Event.UserId,
                    USGenderName = (aeu.Event.User.Gender.GenderName), USCityName = (aeu.Event.User.City.CityName),
                    aeu.Event.User, aeu.Event.Status , aeu.AccedNotifications
                }).Distinct().OrderByDescending(e => e.Id).ToListAsync();

           // PaginationHelper.ReversEventList(acceptUserEvent);

            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = acceptUserEvent
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = acceptUserEvent.Count();
            var pagedReponse =
                PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
            return Ok(pagedReponse);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("ERROR");
    }

    [Authorize]
    [HttpDelete("сancelEvent/{id}")]
    public async Task<ActionResult<UserEvent>> DeleteUserEvent(int id)
    {
        try
        {
            var userEvent = await _context.UserEvent.Where(ue=> ue.EventId == id && ue.UserId == GetUserId())
                .FirstAsync(ue=> ue.EventId == id && ue.UserId == GetUserId());
            if (userEvent.EventId != id)
            {
                return BadRequest("System errors !!!");
            }
            _context.UserEvent.Remove(userEvent);
            await _context.SaveChangesAsync();
            return Ok(userEvent);

        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest();
    }
    
    
    [Authorize]
    [HttpPost("accedNotification/{id}")]
    public async Task<ActionResult<AccedEventUser>> AccedNotification(int id, string? reportingNotification)
    {
        try
        {
            //var report = _context.AccedReporting.FirstOrDefault(u => u.UserId == GetUserId());
            var acced = await _context.AccedEventUser.FirstOrDefaultAsync(a => a.Id == id);
           
            if (acced != null)
            {
                var  report = new AccedReporting()
                {
                    EventId = acced.EventId,
                    UserId = GetUserId(),
                    TimeCreat = DateTime.Now,
                    ReportingsNotification = reportingNotification
                };
                var accedlist = await _context.AccedReporting.ToListAsync();
                foreach (var list in accedlist)
                {
                    if (list.UserId == GetUserId() && acced.EventId == list.EventId)
                    {
                        return BadRequest("Вы уже поделились отзывом на встречу !!!");
                    }
                }
                acced.AccedNotifications = Models.AccedNotification.Verified;
                _context.AccedReporting.Add(report);
                await _context.SaveChangesAsync();
                return Ok(report);
            }
        }
        catch (ApplicationException e)
        {
            return BadRequest("Systems Errors !!!");
        }
        return BadRequest("Systems Errors !!!");
    }
    
    
    [Authorize]
    [HttpPost("inviteUser/{eventId}")]
    public async Task<IActionResult>  InviteUser(int eventId, int userId)
    {
        try
        {
            var events = _context.Events.FirstOrDefault(e => e.Id == eventId);
            var users = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (events.UserId == GetUserId())
            {
                if (events != null)
                {
                    var userName = _context.Users.FirstOrDefault(u => u.Id == events.UserId);
                        var mPlace = _context.MeatingPlace.FirstOrDefault(p => p.Id == events.MeatingPlaceId);

                        NotificationModel notificationModel = new NotificationModel()
                        {
                            UserId = userId,
                            DeviceId = PasswordGeneratorService.OffHesh(users.DeviceId),
                            IsAndroiodDevice = true,
                            Title = "Вас пригласили на встречу",
                            Body = $"Уважаемый Connectёр! \n" +
                                   $"+{userName.Username} приглашает вас на встречу \n " +
                                   $"по обьявлению \n " +
                                   $"{mPlace.NameMeatingPlace} от {events.TimeStart.Value.Date.ToString("dd/MM")} с {events.TimeStart.Value.ToString("HH:mm")} по {events.TimeStart.Value.Date.ToString("dd/MM")} до {events.TimeFinish.Value.ToString("HH:mm")}. \n " +
                                   $"Ознакомиться с условиями обьявления можно  \n " +
                                   $"в его профиле \n " +
                                   $"Скопируйте его никнейм и введите в 'Поиск' в \n " +
                                   $"строку поиска по никнейму: \n " +
                                   $"+{userName.Username}",
                                   DateTime = DateTime.Now,
                            Status = false
                        };

                        await _notificationService.SendNotification(notificationModel);
                        _context.NotificationModel.Add(new NotificationModel(notificationModel.Id,
                            notificationModel.UserId, notificationModel.IsAndroiodDevice, notificationModel.Title,
                            notificationModel.Body, notificationModel.DateTime, notificationModel.Status));
                        await _context.SaveChangesAsync();

                        return Ok("Connectёру будет отправлено Пуш уведомление с предложением Отправить заявку на ваше выбранное объявление. \n" +
                                  "Следите за поступающими заявками и не забудьте принять приглашённого Connectёра.!!!");
                    }
                }

            return BadRequest("ERROR");
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }
        
    }
}