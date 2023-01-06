using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        try
        {
            DateTime now = DateTime.Now;
            var list = await (from ev in _context.UserEvent
                from u in _context.Users
                from e in _context.Events
                where ev.UserId == u.Id
                where ev.User.Id == ev.UserId
                where e.Status == Status.Expectations || e.Status == Status.Canceled
                where e.TimeStart >= now
                orderby ev
                select new { ev.Id, ev.User, ev.Event }).Distinct().ToListAsync();

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
    public async Task<IActionResult> RequestEvent(int eventId)
    {
        try
        {
            var events = _context.Events.FirstOrDefault(e => e.Id == eventId);
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
                    where ev.User.Id == ev.UserId
                    where e.Status == Status.Expectations || e.Status == Status.Canceled
                    where e.TimeStart >= now
                    orderby ev
                    select new { ev.Id, ev.User, ev.Event }).Distinct();

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
    [HttpGet("listRequestUser")]
    public async Task<IActionResult> ListRequestUser()
    {
        try
        {
            var list = await (from ev in _context.UserEvent
                    where ev.UserId == GetUserId()
                    select new { ev.Id, ev.User, ev.Event }
                ).Distinct().ToListAsync();
            return Ok(list);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("ERROR !!!");
    }

    [Authorize]
    [HttpGet("listRequestEvent/{id}")]
    public async Task<IActionResult> ListRequestEvent(int id)
    {
        try
        {
            var events = _context.Events.FirstOrDefault(e => e.Id == id);
            if (events != null)
            {
                var list = await (from ev in _context.UserEvent
                        where ev.EventId == events.Id
                        select new { ev.Id, ev.User, ev.Event }
                    ).Distinct().ToListAsync();
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
                            select new { ev.Id, ev.User.PhoneNumber }).Distinct();

                        /*var userEvent = (from ev in _context.UserEvent
                            from u in _context.Users
                            from e in _context.Events
                            where ev.UserId == userId
                            where ev.EventId == eventId
                            where ev.User.Id == ev.UserId
                            where e.Status == Status.Accepted
                            orderby ev
                            select new { ev.Id, ev.User, ev.Event }).ToList().Distinct();*/

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
                            _context.UserEvent.Remove(uE);
                        await _context.SaveChangesAsync();

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

    [Authorize]
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
    }

    [Authorize]
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
    }
}