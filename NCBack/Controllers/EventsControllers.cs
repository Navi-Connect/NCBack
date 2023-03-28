using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;
using NCBack.Dtos.Event;
using NCBack.Filter;
using NCBack.Helpers;
using NCBack.Models;
using NCBack.Services;

namespace NCBack.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsControllers : Controller
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUriService _uriService;

    public EventsControllers(DataContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _uriService = uriService;
    }


    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
        .FindFirstValue(ClaimTypes.NameIdentifier));


    [HttpGet("events")]
    public async Task<IActionResult> Events([FromQuery] ObjectPaginationFilter? filter)
    {
        DateTime now = DateTime.Now;
        var list = (from e in _context.Events
            from u in _context.Users
            where e.UserId == u.Id
            where e.AimOfTheMeetingId == e.AimOfTheMeeting.Id
            where e.MeetingCategoryId == e.MeetingCategory.Id
            where e.MeatingPlaceId == e.MeatingPlace.Id
            /*where e.MainСategories.Where(x=>! e.MyInterestsId.Contains(x.MyInterestsId)) == e.MyInterests.Select(i=>i.Id).ToList()*/
            /*where e.MyInterests == e.MyInterests.Where(m => !e.MyInterestsId.Contains(m.Id)).ToList()*/
            where e.Status == Status.Expectations || e.Status == Status.Canceled
            where e.TimeStart >= now
            select new
            {
                e.Id, e.AimOfTheMeeting, e.MeetingCategory, e.MeatingPlace,
                e.TimeStart, e.TimeFinish, e.City, e.Gender,
                e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                e.Interests, e.UserId, e.User, e.Status
            }).Distinct();

        var route = Request.Path.Value;
        var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
        var pagedData = await list
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();
        pagedData.Reverse();
        var totalRecords = await list.CountAsync();
        var pagedReponse =
            PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriService, route);
        return Ok(pagedReponse);
    }

    [Authorize]
    [HttpGet("event/{id}")]
    public async Task<IActionResult> Event(int id)
    {
        var events = await _context.Events.FindAsync(id);
        if (events != null)
        {
            await _context.AimOfTheMeeting.FirstOrDefaultAsync(a => a.Id == events.AimOfTheMeetingId);
            await _context.MeetingCategory.FirstOrDefaultAsync(m => m.Id == events.MeetingCategoryId);
            await _context.MeatingPlace.FirstOrDefaultAsync(m => m.Id == events.MeatingPlaceId);
            await _context.CityList.FirstOrDefaultAsync(c => c.Id == events.CityId);
            await _context.GenderList.FirstOrDefaultAsync(g => g.Id == events.GenderId);
            events.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == events.UserId);
            events.User.Gender = await _context.GenderList.FirstOrDefaultAsync(u => u.Id == events.User.GenderId);
            return Ok(events);
        }

        return BadRequest("Error !!!");
    }

    [Authorize]
    [HttpPost("createEvent")]
    public async Task<ActionResult<Event>> CreateEvent([FromForm] EventCreateDto request)
    {
        DateTime now = DateTime.Now;
        var events = _context.Events.FirstOrDefault(u => u.UserId == GetUserId());

        if (request.TimeStart >= now)
        {
            if (request.TimeFinish >= request.TimeStart)
            {
                var eventsList = await _context.Events.Where(u => u.UserId == GetUserId()).ToListAsync();
                foreach (var list in eventsList)
                {
                    if ((list.TimeStart <= request.TimeFinish && list.TimeFinish >= request.TimeStart) || 
                        (request.TimeStart  <= list.TimeFinish && request.TimeFinish >= list.TimeStart))
                    {
                        return BadRequest( "При создании новых событий в заданный временной промежуток необходимо убедиться, что каждое новое событие не пересекается со временем уже существующих событий, а также что время начала каждого события меньше времени окончания, и время начала всего промежутка меньше времени окончания !!!");
                    }
                    
                }
                
                events = new Event()
                {
                    UserId = GetUserId(),
                    AimOfTheMeetingId = request.AimOfTheMeetingId,
                    AimOfTheMeeting = _context.AimOfTheMeeting.FirstOrDefault(a => a.Id == request.AimOfTheMeetingId),
                    MeetingCategoryId = request.MeetingCategoryId,
                    MeetingCategory = _context.MeetingCategory.FirstOrDefault(m => m.Id == request.MeetingCategoryId),
                    MeatingPlaceId = request.MeatingPlaceId,
                    MeatingPlace = _context.MeatingPlace.FirstOrDefault(m => m.Id == request.MeatingPlaceId),
                    IWant = request.IWant,
                    TimeStart = request.TimeStart,
                    TimeFinish = request.TimeFinish,
                    CreateAdd = DateTime.Now,
                    CityId = request.CityId,
                    City = _context.CityList.FirstOrDefault(c => c.Id == request.CityId),
                    AgeTo = request.AgeTo,
                    AgeFrom = request.AgeFrom,
                    GenderId = request.GenderId,
                    Gender = _context.GenderList.FirstOrDefault(g => g.Id == request.GenderId),
                    CaltulationType = request.CaltulationType,
                    CaltulationSum = request.CaltulationSum,
                    LanguageCommunication = request.LanguageCommunication,
                    Interests = request.Interests,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    User = _context.Users.FirstOrDefault(u =>
                        u.Id == GetUserId() && u.CityId == u.City.Id && u.GenderId == u.Gender.Id),
                    Status = Status.Expectations,
                };

                _context.Events.Add(events);
                await _context.SaveChangesAsync();
                return Ok(events);
            }
        }

        return BadRequest("Ошибка по времении или еще что то !!! ");
    }

    [Authorize]
    [HttpPut("updateEvent/{id}")]
    public async Task<ActionResult<Event>> UpdateEvent([FromForm] EventUpdateDto request, int? id)
    {
        var events = _context.Events.FirstOrDefault(e => e.Id == id);

        if (events == null)
            return BadRequest("Hero not found.");

        events.UserId = GetUserId();
        events.AimOfTheMeetingId = request.AimOfTheMeetingId;
        events.AimOfTheMeeting = _context.AimOfTheMeeting.FirstOrDefault(a => a.Id == request.AimOfTheMeetingId);
        events.MeetingCategoryId = request.MeetingCategoryId;
        events.MeetingCategory = _context.MeetingCategory.FirstOrDefault(m => m.Id == request.MeetingCategoryId);
        events.MeatingPlaceId = request.MeatingPlaceId;
        events.MeatingPlace = _context.MeatingPlace.FirstOrDefault(m => m.Id == request.MeatingPlaceId);
        events.IWant = request.IWant;
        events.TimeStart = request.TimeStart;
        events.TimeFinish = request.TimeFinish;
        events.CreateAdd = DateTime.Now;
        events.CityId = request.CityId;
        events.City = _context.CityList.FirstOrDefault(c => c.Id == request.CityId);
        events.AgeTo = request.AgeTo;
        events.AgeFrom = request.AgeFrom;
        events.GenderId = request.GenderId;
        events.Gender = _context.GenderList.FirstOrDefault(g => g.Id == request.GenderId);
        events.CaltulationType = request.CaltulationType;
        events.CaltulationSum = request.CaltulationSum;
        events.LanguageCommunication = request.LanguageCommunication;
        events.Interests = request.Interests;
        events.Latitude = request.Latitude;
        events.Longitude = request.Longitude;
        events.User = _context.Users.FirstOrDefault(u => u.Id == GetUserId());
        events.User.Gender = _context.GenderList.FirstOrDefault(u => u.Id == events.User.GenderId);

        _context.Events.Update(events);
        await _context.SaveChangesAsync();
        return Ok(events);
    }

    [Authorize]
    [HttpDelete("deleteEvent/{id}")]
    public async Task<ActionResult<Event>> DeleteEvent(int id)
    {
        var events = await _context.Events.FindAsync(id);
        if (events == null)
            return BadRequest("Event not found.");

        _context.Events.Remove(events);
        await _context.SaveChangesAsync();

        return Ok(events);
    }
}