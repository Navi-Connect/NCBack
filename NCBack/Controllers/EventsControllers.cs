using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NCBack.Data;
using NCBack.Dtos.Event;
using NCBack.Models;
using NCBack.Services;

namespace NCBack.Controllers;

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
    
}