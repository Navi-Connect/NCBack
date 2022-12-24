using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;
using NCBack.Dtos.Event;
using NCBack.Models;

namespace NCBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsSearchControllers : Controller
    {
        private readonly DataContext _context;
        
        public EventsSearchControllers(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet("GetSearchEvents")]  
        public  async Task<ActionResult<List<Event>>> GetSearchEvents(
            int? AimOfTheMeetingId = null, int? MeetingCategoryId = null, 
            string? city = "", string? gender = "", int? before = null,int? after = null ,DateTime? time = null)
        { 
            DateTime now = DateTime.Now;
           var lists = (from e in _context.Events
               where e.Status == Status.Expectations || e.Status == Status.Canceled
               where e.TimeStart >= now
               select new
               {
                   e.Id,e.AimOfTheMeetingId ,e.AimOfTheMeeting, e.MeetingCategory, e.MeatingPlace,
                   e.TimeStart, e.TimeFinish, e.City, e.Gender,
                   e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                   e.Interests, e.UserId, e.User, e.Status
               }).Distinct();
           
           if (AimOfTheMeetingId != null)
           {
               lists = (from e in _context.Events
                   where e.AimOfTheMeetingId == AimOfTheMeetingId
                   where e.Status == Status.Expectations || e.Status == Status.Canceled
                   where e.TimeStart >= now
                   select new
                   {
                       e.Id,e.AimOfTheMeetingId ,e.AimOfTheMeeting, e.MeetingCategory, e.MeatingPlace,
                       e.TimeStart, e.TimeFinish, e.City, e.Gender,
                       e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                       e.Interests, e.UserId, e.User, e.Status
                   }).Distinct();
           }

           if (AimOfTheMeetingId != null && MeetingCategoryId != null)
           {
               lists = (from e in _context.Events
                   where e.AimOfTheMeetingId == AimOfTheMeetingId
                   where e.MeetingCategoryId == MeetingCategoryId
                   where e.Status == Status.Expectations || e.Status == Status.Canceled
                   where e.TimeStart >= now
                   select new
                   {
                       e.Id,e.AimOfTheMeetingId ,e.AimOfTheMeeting, e.MeetingCategory, e.MeatingPlace,
                       e.TimeStart, e.TimeFinish, e.City, e.Gender,
                       e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                       e.Interests, e.UserId, e.User, e.Status
                   }).Distinct();
           }
            //Только Город
            if (city != "" && gender == "" && before == null && after == null && time == null)
            {
                var events = await lists.Where(e => e.City == city).ToListAsync();
                return Ok(events);
            }
            
            //пол и город
            if (city != "" && gender != "" && before == null && after == null && time == null )
            {
                var events = await lists.Where(e => e.City == city && e.Gender == gender).ToListAsync();
                return Ok(events);
            }
            //пол город и возраст от
            if (city != "" && gender != "" && before != null && after == null && time == null )
            {
                var events = await lists.Where(e => e.City == city && e.Gender == gender && e.AgeFrom >= before).ToListAsync();
                return Ok(events);
            }
            //пол город и возраст до
            if (city != "" && gender != "" && before == null && after != null && time == null )
            {
                var events = await lists.Where(
                    e => e.City == city && e.Gender == gender && e.AgeTo <= after).ToListAsync();
                return Ok(events);
            }
            //Всё кроме время
            if (city != "" && gender != "" && before != null && after != null && time == null )
            {
                var events = await lists.Where(
                    e => e.City == city && e.Gender == gender && e.AgeFrom >= before && e.AgeTo <= after).ToListAsync();
                return Ok(events);
            }
            //Только гендр
            if (city == "" && gender != "" & before == null && after == null && time == null )
            {
                var events = await lists.Where(
                    e =>  e.Gender == gender ).ToListAsync();
                return Ok(events);
            }
            
            //Только время 
            if (city == "" && gender == "" && before == null && after == null && time != null )
            {

                var events = await lists.Where(
                    e =>  e.TimeStart.Value.Date == time ).ToListAsync();
                return Ok(events);
            }
            
            // Город и время
            if (city != "" && gender == "" && before == null && after == null && time != null )
            {
                var events = await lists.Where(
                    e =>  e.TimeStart.Value.Date == time && e.City == city).ToListAsync();
                return Ok(events);
            }
            // Город и время и гендер
            if (city != "" && gender != "" && before == null && after == null && time != null )
            {
                var events = await lists.Where(
                    e =>  e.TimeStart.Value.Date == time && e.City == city && e.Gender == gender).ToListAsync();
                return Ok(events);
            }
            // Город и время и гендер и время от
            if (city != "" && gender != "" && before != null && after == null && time != null )
            {
                var events = await lists.Where(
                    e =>  e.TimeStart.Value.Date == time && e.City == city && e.Gender == gender && e.AgeFrom >= before).ToListAsync();
                return Ok(events); 
            }
            // Город и время и гендер и время до
            if (city != "" && gender != "" && before == null && after != null && time != null )
            {
                var events = await lists.Where(
                    e =>  e.TimeStart.Value.Date == time && e.City == city && e.Gender == gender && e.AgeTo <= after).ToListAsync();
                return Ok(events);
            }
            //Город и время,  возраст от
            if (city != "" && gender == "" && before != null && after == null && time != null )
            {
                var events = await lists.Where(
                    e =>  e.TimeStart.Value.Date == time && e.City == city && e.AgeFrom >= before).ToListAsync();
                return Ok(events);
            }
            //Город и время,  возраст до
            if (city != "" && gender == "" && before == null && after != null && time != null )
            {
                var events = await lists.Where(
                    e =>  e.TimeStart.Value.Date == time && e.City == city && e.AgeTo <= after).ToListAsync();
                return Ok(events);
            }
            ////Время и ОТ
            if (city == "" && gender == "" && before != null && after == null && time != null )
            {
                var events = await lists.Where(
                    e =>  e.TimeStart.Value.Date == time  && e.AgeFrom >= before).ToListAsync();
                return Ok(events);
            }
            
            //Время и ДО
            if (city == "" && gender == "" && before == null && after != null && time != null )
            {
                var events = await lists.Where(
                    e =>  e.TimeStart.Value.Date == time  && e.AgeTo <= after).ToListAsync();
                return Ok(events);
            }
            //Гендр и время
            if (city == "" && gender != "" && before == null && after == null && time != null )
            {
                var events = await lists.Where(
                    e =>  e.TimeStart.Value.Date == time && e.Gender == gender).ToListAsync();
                return Ok(events);
            }
            
            ////Гендр ОТ
            if (gender != "" && city == "" && before != null && after == null && time == null )
            {
                var events = await lists.Where(
                    e =>  e.AgeFrom >= before && e.Gender == gender).ToListAsync();
                return Ok(events);
            }
            //Гендр и возраст только до
            if (gender != "" && city == "" && before != null && after != null && time == null )
            {
                var events = await lists.Where(
                    e =>  e.AgeFrom <= after && e.Gender == gender).ToListAsync();
                return Ok(events);
            }
            //гендр и возраст от до
            if (gender != "" && city == "" && before != null && after != null && time == null )
            {
                var events = await lists.Where(
                    e =>  e.AgeFrom <= after && e.AgeTo >= before &&e.Gender == gender).ToListAsync();
                return Ok(events);
            }
            //Город и возраст от
            if (gender == "" && city != "" && before != null && after == null && time == null )
            {
                var events = await lists.Where(
                    e =>  e.City == city && e.AgeFrom >= before).ToListAsync();
                return Ok(events);
            }
            //Город и возраст от до
            if (gender == "" && city != "" & before != null && after != null && time == null )
            {
                var events = await lists.Where(
                    e =>  e.City == city && e.AgeFrom >= before && e.AgeTo <= after).ToListAsync();
                return Ok(events);
            }
            //возраст от
            if (gender == "" && city == "" && before != null && after == null && time == null )
            {
                var events = await lists.Where(
                    e => e.AgeFrom >= before ).ToListAsync();
                return Ok(events);
            }
            //от и до
            if (gender == "" && city == "" & before != null && after != null && time == null )
            {
                var events = await lists.Where(
                    e =>  e.AgeFrom >= before && e.AgeTo <= after).ToListAsync();
                return Ok(events);
            }
            ////Всё
            if (city != "" && gender != "" && before != null && after != null && time != null )
            {
                var events = await lists.Where(
                    e =>  e.City == city && e.Gender == gender && e.TimeStart.Value.Date == time &&e.AgeFrom >= before && e.AgeTo <= after).ToListAsync();
                return Ok(events);
            }
            //если пусто
            if (city == "" && gender == "" && before == null && after == null && time == null)
            {
                var events = await lists.ToListAsync();
                return Ok(events);
            }
            
            return BadRequest("Not Found");
            
        }

     
    }
}

