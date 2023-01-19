using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;
using NCBack.Filter;
using NCBack.Helpers;
using NCBack.Models;
using NCBack.Services;


namespace NCBack.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsSearchControllers : Controller
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventsSearchControllers(DataContext context, IUriService uriService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _uriService = uriService;
            _httpContextAccessor = httpContextAccessor;
        }
        
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet("GetSearchEvents")]
        public async Task<ActionResult<List<Event>>>
            GetSearchEvents([FromQuery] PaginationFilter? filter = null, int? AimOfTheMeetingId = null,
                int? MeetingCategoryId = null)
        {
            DateTime now = DateTime.Now;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            /*var events = _context.Events.Find();
            var user = _context.Users.FirstOrDefault(u => u.Id == events.UserId);*/
            var lists = await
                /*_context.Events
                .Where(e => e.Status == Status.Expectations || e.Status == Status.Canceled)
                .Where(e => e.UserId == user.Id)
                .Distinct().ToList();*/
                (from e in _context.Events
                    where e.Status == Status.Expectations || e.Status == Status.Canceled
                    //where e.TimeStart >= now
                    select new
                    { 
                        e.Id, e.AimOfTheMeetingId, e.AimOfTheMeeting, e.MeetingCategoryId, e.MeetingCategory, e.MeatingPlaceId, e.MeatingPlace,
                        e.IWant,e.TimeStart, e.TimeFinish, e.CreateAdd, e.CityId, e.City, e.GenderId, e.Gender,
                        e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                        e.Interests, e.Latitude, e.Longitude, e.UserId, e.User, e.Status
                    }).Distinct().ToListAsync();

            if (AimOfTheMeetingId != null)
            {
                lists = await
                    /*_context.Events
                        .Where(e => e.Status == Status.Expectations || e.Status == Status.Canceled)
                        .Where(e => e.UserId == e.User.Id)
                        /*.Select(u => u.User)#1#
                        .Distinct().ToList();*/
                    (from e in _context.Events
                        where e.AimOfTheMeetingId == AimOfTheMeetingId
                        where e.Status == Status.Expectations || e.Status == Status.Canceled
                        //where e.TimeStart >= now
                        select new
                        {
                            e.Id, e.AimOfTheMeetingId, e.AimOfTheMeeting, e.MeetingCategoryId, e.MeetingCategory, e.MeatingPlaceId, e.MeatingPlace,
                            e.IWant,e.TimeStart, e.TimeFinish, e.CreateAdd, e.CityId, e.City, e.GenderId, e.Gender,
                            e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                            e.Interests, e.Latitude, e.Longitude, e.UserId, e.User, e.Status
                        }).Distinct().ToListAsync();
            }

            if (AimOfTheMeetingId != null && MeetingCategoryId != null)
            {
                lists = await
                    (from e in _context.Events
                        where e.AimOfTheMeetingId == AimOfTheMeetingId && e.MeetingCategoryId == MeetingCategoryId
                        where e.Status == Status.Expectations || e.Status == Status.Canceled
                        //where e.TimeStart >= now
                        select new
                        {
                            e.Id, e.AimOfTheMeetingId, e.AimOfTheMeeting, e.MeetingCategoryId, e.MeetingCategory, e.MeatingPlaceId, e.MeatingPlace,
                            e.IWant,e.TimeStart, e.TimeFinish, e.CreateAdd, e.CityId, e.City, e.GenderId, e.Gender,
                            e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                            e.Interests, e.Latitude, e.Longitude, e.UserId, e.User, e.Status
                        }).Distinct().ToListAsync();

                /*_context.Events
                    .Where(e => e.Status == Status.Expectations || e.Status == Status.Canceled)
                    .Where(e => e.UserId == e.User.Id)
                    /*.Select(u => u.User)#1#
                    .Distinct().ToList();*/
            }
            
            /*var lists = await
                (from e in _context.Events
                    where e.AimOfTheMeetingId == AimOfTheMeetingId && e.MeetingCategoryId == MeetingCategoryId
            
                    //where e.TimeStart >= now
                    select new
                    {
                        e.Id, e.AimOfTheMeetingId, e.AimOfTheMeeting, e.MeetingCategoryId, e.MeetingCategory, e.MeatingPlaceId, e.MeatingPlace,
                        e.IWant,e.TimeStart, e.TimeFinish, e.CreateAdd, e.CityId, e.City, e.GenderId, e.Gender,
                        e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                        e.Interests, e.Latitude, e.Longitude, e.UserId, e.User, e.Status
                    }).Distinct().ToListAsync();

            if (AimOfTheMeetingId != null)
            {
                lists = await
                    (from e in _context.Events
                        where e.AimOfTheMeetingId == AimOfTheMeetingId && e.MeetingCategoryId == MeetingCategoryId
                        where e.Status == Status.Expectations || e.Status == Status.Canceled
                    
                        //where e.TimeStart >= now
                        select new
                        {
                            e.Id, e.AimOfTheMeetingId, e.AimOfTheMeeting, e.MeetingCategoryId, e.MeetingCategory, e.MeatingPlaceId, e.MeatingPlace,
                            e.IWant,e.TimeStart, e.TimeFinish, e.CreateAdd, e.CityId, e.City, e.GenderId, e.Gender,
                            e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                            e.Interests, e.Latitude, e.Longitude, e.UserId, e.User, e.Status ,
                        }).Distinct().ToListAsync();
            }

            if (AimOfTheMeetingId != null && MeetingCategoryId != null)
            {
                lists = await
                    (from e in _context.Events
                        where e.AimOfTheMeetingId == AimOfTheMeetingId && e.MeetingCategoryId == MeetingCategoryId
                        where e.Status == Status.Expectations || e.Status == Status.Canceled
                        //where e.TimeStart >= now
                        select new
                        {
                            e.Id, e.AimOfTheMeetingId, e.AimOfTheMeeting, e.MeetingCategoryId, e.MeetingCategory, e.MeatingPlaceId, e.MeatingPlace,
                            e.IWant,e.TimeStart, e.TimeFinish, e.CreateAdd, e.CityId, e.City, e.GenderId, e.Gender,
                            e.AgeTo, e.AgeFrom, e.CaltulationType, e.CaltulationSum, e.LanguageCommunication,
                            e.Interests, e.Latitude, e.Longitude, e.UserId, e.User, e.Status 
                        }).Distinct().ToListAsync();
            }
            */

            //если пусто
            if (filter.CityId == null && filter.GenderId == null &&
                filter.Year == null && filter.Month == null && filter.Date == null)
            {
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.Year, filter.Month, filter.Date);
                var pagedData = lists
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize).ToList();
                var totalRecords = await _context.Events.CountAsync();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }


            //Только Город
            if (filter.CityId != null && filter.GenderId == null && 
                (filter.Year == null && filter.Month == null && filter.Date == null))
            {
                var events = lists.Where(e => e.CityId == filter.CityId).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }

            /*//Город до
            if (filter.CityId != null && filter.GenderId == null  &&
                (filter.Year == null && filter.Month == null && filter.Date == null))
            {
                var events = lists.Where(e => e.CityId == filter.CityId && e.AgeTo <= filter.AgeTo).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }*/

            //пол и город
            if (filter.CityId != null && filter.GenderId != null  &&
                (filter.Year == null && filter.Month == null && filter.Date == null))
            {
                var events = lists.Where(e => e.CityId == filter.CityId 
                                              && e.GenderId == filter.GenderId && e.Gender.GenderName == "М/Ж").ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }

            /*//пол город и возраст от
            if (filter.CityId != null && filter.GenderId != null && filter.AgeFrom != null && filter.AgeTo == null &&
                (filter.Year == null && filter.Month == null && filter.Date == null)) //AgeTo = до -- --AgeFrom  от
            {
                var events = lists.Where(e =>
                    e.CityId == filter.CityId && e.GenderId == filter.GenderId && e.AgeFrom >= filter.AgeFrom).ToList();

                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }*/

            /*//пол город и возраст до
            if (filter.CityId != null && filter.GenderId != null && filter.AgeFrom == null && filter.AgeTo != null &&
                (filter.Year == null && filter.Month == null && filter.Date == null))
            {
                var events = lists.Where(
                        e => e.CityId == filter.CityId && e.GenderId == filter.GenderId && e.AgeTo <= filter.AgeTo)
                    .ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }*/

            /*//Всё кроме время
            if (filter.CityId != null && filter.GenderId != null && filter.AgeFrom != null && filter.AgeTo != null &&
                (filter.Year == null && filter.Month == null && filter.Date == null))
            {
                var events = lists.Where(
                    e => e.CityId == filter.CityId && e.GenderId == filter.GenderId && e.AgeFrom >= filter.AgeFrom &&
                         e.AgeTo <= filter.AgeTo).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }*/

            //Только гендр
            if (filter.CityId == null && filter.GenderId != null && 
                (filter.Year == null && filter.Month == null && filter.Date == null))
            {
                var events = lists.Where(
                    e => e.User.GenderId == filter.GenderId && e.GenderId == user.GenderId 
                                                            && e.Gender.GenderName == "М/Ж").ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = await _context.Events.Where(
                    e => e.GenderId == filter.GenderId).CountAsync();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }

            //Только время 
            if (filter.CityId == null && filter.GenderId == null && 
                (filter.Year != null && filter.Month != null && filter.Date != null))
            {
                var events = lists.Where(
                        e => e.TimeStart.Value.Date.ToString("yyyy-M-d") ==
                             $"{filter.Year}-{filter.Month}-{filter.Date}")
                    .ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }

            // Город и время
            if (filter.CityId != null && filter.GenderId == null && 
                (filter.Year != null && filter.Month != null && filter.Date != null))
            {
                var events = lists.Where(
                    e => e.TimeStart.Value.Date.ToString("yyyy-M-d") == $"{filter.Year}-{filter.Month}-{filter.Date}" &&
                         e.CityId == filter.CityId).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }

            // Город и время и гендер
            if (filter.CityId != null && filter.GenderId != null &&
                (filter.Year != null && filter.Month != null && filter.Date != null))
            {
                var events = lists.Where(
                    e => e.TimeStart.Value.Date.ToString("yyyy-M-d") == $"{filter.Year}-{filter.Month}-{filter.Date}" &&
                         e.CityId == filter.CityId && e.User.GenderId == filter.GenderId 
                         && e.GenderId == user.GenderId && e.Gender.GenderName == "М/Ж").ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }

            /*// Город и время и гендер и время от
            if (filter.CityId != null && filter.GenderId != null && filter.AgeTo == null && filter.AgeFrom != null &&
                (filter.Year != null && filter.Month != null && filter.Date != null))
            {
                var events = lists.Where(
                    e => e.TimeStart.Value.Date.ToString("yyyy-M-d") == $"{filter.Year}-{filter.Month}-{filter.Date}" &&
                         e.CityId == filter.CityId &&
                         e.GenderId == filter.GenderId && e.AgeFrom >= filter.AgeFrom).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }*/

            /*// Город и время и гендер и время до
            if (filter.CityId != null && filter.GenderId != null && filter.AgeTo != null && filter.AgeFrom == null &&
                (filter.Year != null && filter.Month != null && filter.Date != null))
            {
                var events = lists.Where(
                        e => e.TimeStart.Value.Date.ToString("yyyy-M-d") ==
                             $"{filter.Year}-{filter.Month}-{filter.Date}" &&
                             e.CityId == filter.CityId && e.GenderId == filter.GenderId && e.AgeTo <= filter.AgeTo)
                    .ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }*/

            /*
            //Город и время,  возраст от
            if (filter.CityId != null && filter.GenderId == null && filter.AgeFrom != null && filter.AgeTo == null &&
                (filter.Year != null && filter.Month != null && filter.Date != null))
            {
                var events = lists.Where(
                    e => e.TimeStart.Value.Date.ToString("yyyy-M-d") == $"{filter.Year}-{filter.Month}-{filter.Date}" &&
                         e.CityId == filter.CityId && e.AgeFrom >= filter.AgeFrom).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }
            */

            /*
            //Город и время,  возраст до
            if (filter.CityId != null && filter.GenderId == null && filter.AgeFrom == null && filter.AgeTo != null &&
                (filter.Year != null && filter.Month != null && filter.Date != null))
            {
                var events = lists.Where(
                    e => e.TimeStart.Value.Date.ToString("yyyy-M-d") == $"{filter.Year}-{filter.Month}-{filter.Date}" &&
                         e.CityId == filter.CityId && e.AgeTo <= filter.AgeTo).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }*/

            /*
            ////Время и ОТ
            if (filter.CityId == null && filter.GenderId == null && filter.AgeFrom != null && filter.AgeTo == null &&
                (filter.Year != null && filter.Month != null && filter.Date != null))
            {
                var events = lists.Where(
                    e => e.TimeStart.Value.Date.ToString("yyyy-M-d") == $"{filter.Year}-{filter.Month}-{filter.Date}" &&
                         e.AgeFrom >= filter.AgeFrom).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }

            //Время и ДО
            if (filter.CityId == null && filter.GenderId == null && filter.AgeFrom == null && filter.AgeTo != null &&
                (filter.Year != null && filter.Month != null && filter.Date != null))
            {
                var events = lists.Where(
                    e => e.TimeStart.Value.Date.ToString("yyyy-M-d") == $"{filter.Year}-{filter.Month}-{filter.Date}" &&
                         e.AgeTo <= filter.AgeTo).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }*/

            //Гендр и время
            if (filter.CityId == null && filter.GenderId != null && 
                (filter.Year != null && filter.Month != null && filter.Date != null))
            {
                var events = lists.Where(
                    e => e.TimeStart.Value.Date.ToString("yyyy-M-d") == $"{filter.Year}-{filter.Month}-{filter.Date}" &&
                         e.User.GenderId == filter.GenderId && e.GenderId == user.GenderId && e.Gender.GenderName == "М/Ж");
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }

            /*
            ////Гендр ОТ
            if (filter.GenderId != null && filter.CityId == null && filter.AgeFrom != null && filter.AgeTo == null &&
                (filter.Year == null && filter.Month == null && filter.Date == null))
            {
                var events = lists.Where(
                    e => e.AgeFrom >= filter.AgeFrom && e.GenderId == filter.GenderId).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }

            //Гендр и возраст только до
            if (filter.GenderId != null && filter.CityId == null && filter.AgeFrom != null && filter.AgeTo != null &&
                (filter.Year == null && filter.Month == null && filter.Date == null))
            {
                var events = lists.Where(
                    e => e.AgeFrom <= filter.AgeTo && e.GenderId == filter.GenderId).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }

            //гендр и возраст от до
            if (filter.GenderId != null && filter.CityId == null && filter.AgeFrom != null && filter.AgeTo != null &&
                (filter.Year == null && filter.Month == null && filter.Date == null))
            {
                var events = lists.Where(
                        e => e.AgeFrom <= filter.AgeTo && e.AgeTo >= filter.AgeFrom && e.GenderId == filter.GenderId)
                    .ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }
            */

            /*//Город и возраст от
            if (filter.GenderId == null && filter.CityId != null && filter.AgeFrom != null && filter.AgeTo == null &&
                (filter.Year == null && filter.Month == null && filter.Date == null))
            {
                var events = lists.Where(
                    e => e.CityId == filter.CityId && e.AgeFrom >= filter.AgeFrom).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }

            //Город и возраст от до
            if (filter.GenderId == null && filter.CityId != null & filter.AgeFrom != null && filter.AgeTo != null &&
                (filter.Year == null && filter.Month == null && filter.Date == null))
            {
                var events = lists.Where(
                    e => e.CityId == filter.CityId && e.AgeFrom >= filter.AgeFrom && e.AgeTo <= filter.AgeTo).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }

            //возраст от
            if (filter.GenderId == null && filter.CityId == null && filter.AgeFrom != null && filter.AgeTo == null &&
                (filter.Year == null && filter.Month == null && filter.Date == null))
            {
                var events = lists.Where(
                    e => e.AgeFrom >= filter.AgeFrom).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }

            //от и до
            if (filter.GenderId == null && filter.CityId == null & filter.AgeFrom != null && filter.AgeTo != null &&
                (filter.Year == null && filter.Month == null && filter.Date == null))
            {
                var events = lists.Where(
                    e => e.AgeFrom >= filter.AgeFrom && e.AgeTo <= filter.AgeTo).ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.AgeFrom, filter.AgeTo, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }*/

            ////Всё
            if (filter.CityId != null && filter.GenderId != null && 
                (filter.Year != null && filter.Month != null && filter.Date != null))
            {
                var events = lists.Where(
                    e => e.CityId == filter.CityId && e.User.GenderId == filter.GenderId && e.GenderId == user.GenderId 
                         && e.Gender.GenderName == "М/Ж" && e.TimeStart.Value.Date.ToString("yyyy-M-d") == $"{filter.Year}-{filter.Month}-{filter.Date}").ToList();
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.CityId,
                    filter.GenderId, filter.Year, filter.Month, filter.Date);
                var pagedData = events
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
                var totalRecords = events.Count();
                var pagedReponse =
                    PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
            }


            return BadRequest("Not Found");
        }
    }
}