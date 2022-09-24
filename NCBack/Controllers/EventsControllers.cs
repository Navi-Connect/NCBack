using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        var events = _context.Events.ToList();
        return Ok(events);
    }
    
    [Authorize]
    [HttpPost("createEvent")]
    public async Task<ActionResult<Event>> CreateEvent([FromForm] EventCreateDto request)
    {
        var events = _context.Events.FirstOrDefault(u => u.UsreId == GetUserId());
       
        events = new Event()
        {
            UsreId = GetUserId(),
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
        };
        
        events.User = _context.Users.FirstOrDefault(u => u.Id == events.UsreId);
        _context.Events.Add(events);
        await _context.SaveChangesAsync();
        return Ok(events);
    }

    [Authorize]
    [HttpPut("updateEvent/{id}")]
    public async Task<ActionResult<Event>> UpdateEvent([FromForm] EventUpdateDto request, int? id)
    {
        var events =  _context.Events.FirstOrDefault(e => e.Id == id);

        if (events == null)
            return BadRequest("Hero not found.");

        events.UsreId = GetUserId();
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

        events.User = _context.Users.FirstOrDefault(u => u.Id == events.UsreId);

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