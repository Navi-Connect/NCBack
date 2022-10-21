using System.Security.Claims;
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
    
    [HttpGet("events")]
    public async Task<IActionResult> Events()
    {
        var list = (from e in _context.Events
            from u in _context.Users
            where e.UserId == u.Id
            select new {e.Id, e.AimOfTheMeeting, e.MeetingCategory, e.MeatingName, 
                e.Date , e.TimeStart, e.TimeFinish, e.City, e.Region, e.Gender,
                e.AgeTo , e.AgeFrom , e.CaltulationType , e.CaltulationSum, e.LanguageCommunication ,
                e.MeatingPlace , e.MeatingInterests , e.UserId ,e.User } ).ToList();
        return Ok(list);
    }

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
        var events = _context.Events.FirstOrDefault(u => u.UserId == GetUserId());

        /*if (request.Date > DateTime.Now || 
            request.TimeStart  < request.TimeFinish  
            || request.TimeStart !> DateTime.Now)
        {*/
            events = new Event()
            { 
                UserId = GetUserId(),
                MeatingName = request.MeatingName,
                AimOfTheMeeting = request.AimOfTheMeeting,
                MeetingCategory = request.MeetingCategory,
                MeatingPlace = request.MeatingPlace,
                Date = request.Date,
                TimeStart = request.TimeStart,
                TimeFinish = request.TimeFinish,
                City = request.City,
                Region = request.Region,
                AgeTo = request.AgeTo,
                AgeFrom = request.AgeFrom,
                Gender = request.Gender,
                CaltulationType = request.CaltulationType,
                CaltulationSum = request.CaltulationSum,
                LanguageCommunication = request.LanguageCommunication,
                MeatingInterests = request.MeatingInterests,
                User = _context.Users.FirstOrDefault(u => u.Id == GetUserId())
            };
        
            _context.Events.Add(events);
            await _context.SaveChangesAsync();
            return Ok(events);
        /*}

        return BadRequest("Ошибка по аремении или еще что то !!! ");*/
    }

    [Authorize]
    [HttpPut("updateEvent/{id}")]
    public async Task<ActionResult<Event>> UpdateEvent([FromForm] EventUpdateDto request, int? id)
    {
        var events =  _context.Events.FirstOrDefault(e => e.Id == id);

        if (events == null)
            return BadRequest("Hero not found.");

        events.UserId = GetUserId();
        events.MeatingName = request.MeatingName;
        events.AimOfTheMeeting = request.AimOfTheMeeting;
        events.MeetingCategory = request.MeetingCategory;
        events.MeatingPlace = request.MeatingPlace;
        events.Date = request.Date;
        events.TimeStart = request.TimeStart;
        events.TimeFinish = request.TimeFinish;
        events.City = request.City;
        events.Region = request.Region;
        events.AgeTo = request.AgeTo;
        events.AgeFrom = request.AgeFrom;
        events.Gender = request.Gender;
        events.CaltulationType = request.CaltulationType;
        events.CaltulationSum = request.CaltulationSum;
        events.LanguageCommunication = request.LanguageCommunication;
        events.MeatingInterests = request.MeatingInterests;
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
            return BadRequest("News not found.");

        _context.Events.Remove(events);
        await _context.SaveChangesAsync();

        return Ok(events);
    }

}