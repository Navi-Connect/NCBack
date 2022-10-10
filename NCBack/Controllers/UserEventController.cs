using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NCBack.Data;
using NCBack.Models;

namespace NCBack.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserEventController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserEventController(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
        .FindFirstValue(ClaimTypes.NameIdentifier));

    [Authorize]
    [HttpGet("listRequest")]
    public async Task<IActionResult> ListRequest()
    {
        var list = (from ev in _context.UserEvent
                from u in _context.Users
                from e in _context.Events
                where ev.UserId == u.Id
                where ev.EventId == e.Id
                select new { ev.Id, ev.UserId, ev.User, ev.EventId, ev.Event }
            ).ToList();
        
        return Ok(list);
    }

    [Authorize]
    [HttpPost("requestEvent")]
    public async Task<IActionResult> RequestEvent(int eventId)
    {
        var events = _context.Events.FirstOrDefault(e => e.Id == eventId);
        var users = _context.Users.FirstOrDefault(u => u.Id != GetUserId());

        var userEvent = _context.UserEvent
            .Where(ue => users != null && ue.UserId == users.Id)
            .Where(eu => events != null && eu.EventId == events.Id)
            .Where(u=> u.User == users)
            .Where(e=>e.Event == events)
            .ToList();

        if (events.Id == eventId || GetUserId() != events.UserId || GetUserId() == users.Id)
        {
            _context.UserEvent.Add(new UserEvent(GetUserId(), eventId));
            await _context.SaveChangesAsync();
        
            return Ok(userEvent);
        }
        
        return BadRequest("Error limit !!! ");
    }
    
    [Authorize]
    [HttpGet("listRequestUser")]
    public async Task<IActionResult> ListRequestUser()
    {
        var list = (from ev in _context.UserEvent
                where ev.UserId == GetUserId()
                select new {ev.Id , ev.UserId , ev.User , ev.EventId, ev.Event}
            ).ToList();
        return Ok(list);
    }
    
    [HttpGet("listRequestEvent/{id}")]
    public async Task<IActionResult> ListRequestEvent(int id)
    {
        var list = (from ev in _context.UserEvent
                where ev.EventId == id
                select new {ev.Id , ev.UserId , ev.User , ev.EventId, ev.Event}
            ).ToList();
        
        return Ok(list);
    }
}