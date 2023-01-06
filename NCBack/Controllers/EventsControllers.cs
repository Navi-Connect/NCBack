using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;
using NCBack.Dtos.Event;
using NCBack.Models;

namespace NCBack.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsControllers : Controller
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EventsControllers(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }


    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
        .FindFirstValue(ClaimTypes.NameIdentifier));

    [Authorize]
    [HttpGet("events")]
    public async Task<IActionResult> Events()
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

        return Ok(list);
    }

    [Authorize]
    [HttpGet("myEventsAccepted")]
    public async Task<IActionResult> EventsAccepteds()
    {
        var list = (from evensAccepted in _context.Events
            from u in _context.Users
            from acceptedUser in _context.UserEvent
            where evensAccepted.UserId == GetUserId()
            where evensAccepted.Status == Status.Accepted
            where acceptedUser.EventId == evensAccepted.Id
            select new { evensAccepted, acceptedUser.UserId, acceptedUser.User }).ToList().Distinct();
        return Ok(list);
    }

    [Authorize]
    [HttpGet("event/{id}")]
    public async Task<IActionResult> Event(int id)
    {
        var events = await _context.Events.FindAsync(id);
        events.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == events.UserId);
        return Ok(events);
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
                events = new Event()
                {
                    UserId = GetUserId(),
                    AimOfTheMeetingId = request.AimOfTheMeetingId,
                    MeetingCategoryId = request.MeetingCategoryId,
                    MeatingPlaceId = request.MeatingPlaceId,
                    IWant = request.IWant,
                    TimeStart = request.TimeStart,
                    TimeFinish = request.TimeFinish,
                    CreateAdd = DateTime.Now,
                    CityId = request.CityId,
                    AgeTo = request.AgeTo,
                    AgeFrom = request.AgeFrom,
                    GenderId = request.GenderId,
                    CaltulationType = request.CaltulationType,
                    CaltulationSum = request.CaltulationSum,
                    LanguageCommunication = request.LanguageCommunication,
                    Interests = request.Interests,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    User = _context.Users.FirstOrDefault(u => u.Id == GetUserId()),
                    Status = Status.Expectations
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
        events.MeetingCategoryId = request.MeetingCategoryId;
        events.MeatingPlaceId = request.MeatingPlaceId;
        events.IWant = request.IWant;
        events.TimeStart = request.TimeStart;
        events.TimeFinish = request.TimeFinish;
        events.CreateAdd = DateTime.Now;
        events.CityId = request.CityId;
        events.AgeTo = request.AgeTo;
        events.AgeFrom = request.AgeFrom;
        events.GenderId = request.GenderId;
        events.CaltulationType = request.CaltulationType;
        events.CaltulationSum = request.CaltulationSum;
        events.LanguageCommunication = request.LanguageCommunication;
        events.Interests = request.Interests;
        events.Latitude = request.Latitude;
        events.Longitude = request.Longitude;
        events.User = _context.Users.FirstOrDefault(u => u.Id == events.UserId);

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