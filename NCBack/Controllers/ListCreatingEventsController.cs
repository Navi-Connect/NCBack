using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;

namespace NCBack.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ListCreatingEventsController : Controller
{
    private readonly DataContext _context;

    public ListCreatingEventsController(DataContext context)
    {
        _context = context;
    }


    [HttpGet("getByIdAimOfTheMeeting/{Id}")]
    public async Task<ActionResult> GetAimOfTheMeeting(int Id)
    {
        var aimOfTheMeeting = await _context.AimOfTheMeeting.FirstOrDefaultAsync(a => a.Id == Id);
        return Ok(aimOfTheMeeting);
    }


    [HttpGet("getAimOfTheMeeting")]
    public async Task<ActionResult> GetAimOfTheMeeting()
    {
        var aimOfTheMeeting = _context.AimOfTheMeeting.ToList().Distinct();
        return Ok(aimOfTheMeeting);
    }

    [HttpGet("getByIdMeetingCategory/{Id}")]
    public async Task<ActionResult> MeetingCategory(int Id)
    {
        var meetingCategory = _context.MeetingCategory.FirstOrDefault(m => m.Id == Id);
        return Ok(meetingCategory);
    }

    [HttpGet("getMeetingCategory")]
    public async Task<ActionResult> MeetingCategory()
    {
        var meetingCategory = _context.MeetingCategory.ToList().Distinct();
        return Ok(meetingCategory);
    }


    [HttpGet("getByIdMeatingPlace/{Id}")]
    public async Task<ActionResult> GetByIdMeatingPlace(int Id)
    {
        var meatingPlace = (from p in _context.MeatingPlace
            from c in _context.MeetingCategory
            where p.Id == Id
            where p.MeetingCategoryId == c.Id
            select new { p.Id, p.NameMeatingPlace, p.MeetingCategoryId, p.MeetingCategory }).Distinct();
        return Ok(meatingPlace);
    }

    [HttpGet("getMeatingPlace")]
    public async Task<ActionResult> MeatingPlace()
    {
        var meatingPlace = (from p in _context.MeatingPlace
            from c in _context.MeetingCategory
            where c.Id == p.MeetingCategoryId
            orderby p
            select new { p.Id, p.NameMeatingPlace, p.MeetingCategoryId, p.MeetingCategory }).ToList().Distinct();
        return Ok(meatingPlace);
    }

    [HttpGet("getMeatingPlace/{Id}")]
    public async Task<ActionResult> MeatingPlace(int Id)
    {
        var list = (from i in _context.MeatingPlace
            from m in _context.MeetingCategory
            where i.MeetingCategoryId == Id
            select new { i.Id, i.NameMeatingPlace, i.MeetingCategoryId, i.MeetingCategory }).ToList().Distinct();
        return Ok(list);
    }


    /*[HttpGet("getByIdMyInterests/{Id}")]
    public async Task<ActionResult> GetByIdMyInterests(int Id)
    {
        var myInterests = _context.MyInterests.FirstOrDefault(m => m.Id == Id);
        return Ok(myInterests);
    }

    [HttpGet("getMyInterests")]
    public async Task<ActionResult> MyInterests()
    {
        var myInterests = _context.MyInterests.ToList().Distinct();
        return Ok(myInterests);
    } 
    
    
    
    
    [HttpGet("getByIdMainСategories/{Id}")]
    public async Task<ActionResult> GetByIdMainСategories(int Id)
    {
        var mainСategories = _context.MainСategories.FirstOrDefault(m => m.Id == Id);
        return Ok(mainСategories);
    } 
    
    
    [HttpGet("getMainСategories")]
    public async Task<ActionResult> GetMainСategories()
    {
        var mainСategories = _context.MainСategories.ToList().Distinct();
        return Ok(mainСategories);
    } 
    
    [HttpGet("getMainСategories/{Id}")]
    public async Task<ActionResult> MainСategories(int Id)
    {
        var list = (from i in _context.MainСategories
            from m in _context.MyInterests
            where i.Id == Id
            select new {i.Id, i.NameMainСategories, i.MyInterestsId, i.MyInterests} ).ToList().Distinct();
        return Ok(list);
    }*/
}