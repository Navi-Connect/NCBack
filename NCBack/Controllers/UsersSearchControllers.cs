using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;
using NCBack.Filter;
using NCBack.Helpers;
using NCBack.Models;
using NCBack.Services;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;


namespace NCBack.Controllers;

public class UsersSearchControllers : Controller
{
    private readonly DataContext _context;
    private readonly IUriService _uriService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UsersSearchControllers(DataContext context, IUriService uriService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _uriService = uriService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
        .FindFirstValue(ClaimTypes.NameIdentifier));

    [Authorize]
    [HttpGet("GetSearchUsers")]
    public async Task<ActionResult<List<User>>> GetGetSearchUsers([FromQuery] PaginationFilter? filterUsers = null, string? userName = null)
    {
        var usersList = await _context.Users.Distinct().OrderByDescending(e => e.Id).ToListAsync();
        List<User> usersResult;
        
        
        // если пусто
        if (userName == null && filterUsers.GenderId == null && filterUsers.CityId == null
            && filterUsers.AgeTo == null && filterUsers.AgeFrom == null)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filterUsers.PageNumber, filterUsers.PageSize, filterUsers.CityId,
                filterUsers.GenderId,  filterUsers.Year, filterUsers.Month, filterUsers.Date, filterUsers.AgeTo, filterUsers.AgeFrom);
            var pagedData = usersList.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();
            var totalRecords = await _context.Users.CountAsync();
            pagedData.Reverse();
            object pagedReponse =
                PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
            
            return Ok(pagedReponse);
        }
        
        ////Только Город
        if (userName == null && filterUsers.GenderId == null && filterUsers.CityId != null
            && filterUsers.AgeTo == null && filterUsers.AgeFrom == null)
        {
            usersResult = usersList.Where(u => u.CityId == filterUsers.CityId).ToList();
            
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filterUsers.PageNumber, filterUsers.PageSize, filterUsers.CityId,
                filterUsers.GenderId,  filterUsers.Year, filterUsers.Month, filterUsers.Date, filterUsers.AgeTo, filterUsers.AgeFrom);
            var pagedData = usersResult.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();
            var totalRecords = await _context.Users.CountAsync();
            
            var pagedReponse =
                PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
            
        }
        //гендр и город
        if (userName == null && filterUsers.GenderId != null && filterUsers.CityId != null
            && filterUsers.AgeTo == null && filterUsers.AgeFrom == null)
        {
            usersResult = usersList.Where(u => u.CityId == filterUsers.CityId && u.GenderId == filterUsers.GenderId).ToList();
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filterUsers.PageNumber, filterUsers.PageSize, filterUsers.CityId,
                filterUsers.GenderId,  filterUsers.Year, filterUsers.Month, filterUsers.Date, filterUsers.AgeTo, filterUsers.AgeFrom);
            var pagedData = usersResult.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();
            var totalRecords = await _context.Users.CountAsync();
            
            var pagedReponse =
                PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
                return Ok(pagedReponse);
        }
        //Только гендр
        if (userName == null && filterUsers.GenderId != null && filterUsers.CityId == null
            && filterUsers.AgeTo == null && filterUsers.AgeFrom == null)
        {
            usersResult = usersList.Where(u => u.GenderId == filterUsers.GenderId).ToList();
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filterUsers.PageNumber, filterUsers.PageSize, filterUsers.CityId,
                filterUsers.GenderId,  filterUsers.Year, filterUsers.Month, filterUsers.Date, filterUsers.AgeTo, filterUsers.AgeFrom);
            var pagedData = usersResult.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();
            var totalRecords = await _context.Users.CountAsync();
            
            var pagedReponse =
                PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }
        
        
        //Возрост До
        if (userName == null && filterUsers.GenderId == null && filterUsers.CityId == null
            && filterUsers.AgeTo != null && filterUsers.AgeFrom == null)
        {
            usersResult = usersList.Where(
                    e => e.CityId == filterUsers.CityId && e.GenderId == filterUsers.GenderId && e.Age <= filterUsers.AgeTo)
                .ToList();
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filterUsers.PageNumber, filterUsers.PageSize, filterUsers.CityId,
                filterUsers.GenderId,  filterUsers.Year, filterUsers.Month, filterUsers.Date, filterUsers.AgeTo, filterUsers.AgeFrom);
            var pagedData = usersResult
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = usersResult.Count();
            var pagedReponse =
                PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        } 
        
        //Возросто ОТ
        if (userName == null && filterUsers.GenderId == null && filterUsers.CityId == null
            && filterUsers.AgeTo == null && filterUsers.AgeFrom != null)
        {
            usersResult = usersList.Where(u =>
                     u.Age <= filterUsers.AgeFrom)
                .ToList();
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filterUsers.PageNumber, filterUsers.PageSize, filterUsers.CityId,
                filterUsers.GenderId,  filterUsers.Year, filterUsers.Month, filterUsers.Date, filterUsers.AgeTo, filterUsers.AgeFrom);
            var pagedData = usersResult.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();
            var totalRecords = await _context.Users.CountAsync();
            
            var pagedReponse =
                PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }
        
        // Город Возрост До
        if (userName == null && filterUsers.GenderId == null && filterUsers.CityId == null
            && filterUsers.AgeTo != null && filterUsers.AgeFrom == null)
        {
            usersResult = usersList.Where(u =>
                    u.CityId == filterUsers.CityId && u.Age >= filterUsers.AgeTo )
                .ToList();
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filterUsers.PageNumber, filterUsers.PageSize, filterUsers.CityId,
                filterUsers.GenderId,  filterUsers.Year, filterUsers.Month, filterUsers.Date, filterUsers.AgeTo, filterUsers.AgeFrom);
            var pagedData = usersResult.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();
            var totalRecords = await _context.Users.CountAsync();
            
            var pagedReponse =
                PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }
        
        // Город Возрост от
        if (userName == null && filterUsers.GenderId == null && filterUsers.CityId == null
            && filterUsers.AgeTo == null && filterUsers.AgeFrom != null)
        {
            usersResult = usersList.Where(u =>
                    u.CityId == filterUsers.CityId && u.Age <= filterUsers.AgeFrom )
                .ToList();
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filterUsers.PageNumber, filterUsers.PageSize, filterUsers.CityId,
                filterUsers.GenderId,  filterUsers.Year, filterUsers.Month, filterUsers.Date, filterUsers.AgeTo, filterUsers.AgeFrom);
            var pagedData = usersResult.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();
            var totalRecords = await _context.Users.CountAsync();
            
            var pagedReponse =
                PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }


        // Возрост от и Возрост До
        if (userName == null && filterUsers.GenderId == null && filterUsers.CityId == null
            && filterUsers.AgeTo != null && filterUsers.AgeFrom != null)
        {
            usersResult = usersList.Where(u =>
                    u.Age >= filterUsers.AgeTo && u.Age <= filterUsers.AgeFrom)
                .ToList();
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filterUsers.PageNumber, filterUsers.PageSize, filterUsers.CityId,
                filterUsers.GenderId,  filterUsers.Year, filterUsers.Month, filterUsers.Date, filterUsers.AgeTo, filterUsers.AgeFrom);
            var pagedData = usersResult.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();
            var totalRecords = await _context.Users.CountAsync();
            
            var pagedReponse =
                PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }
        // Город Возрост от и Возрост До
        if (userName == null && filterUsers.GenderId == null && filterUsers.CityId != null
            && filterUsers.AgeTo != null && filterUsers.AgeFrom != null)
        {
            usersResult = usersList.Where(u =>
                    u.Age >= filterUsers.AgeTo && u.Age <= filterUsers.AgeFrom && filterUsers.CityId == u.CityId)
                .ToList();
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filterUsers.PageNumber, filterUsers.PageSize, filterUsers.CityId,
                filterUsers.GenderId,  filterUsers.Year, filterUsers.Month, filterUsers.Date, filterUsers.AgeTo, filterUsers.AgeFrom);
            var pagedData = usersResult.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();
            var totalRecords = await _context.Users.CountAsync();
            
            var pagedReponse =
                PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }
        // Город Возрост от и Возрост До Гедр
        
        if (userName == null && filterUsers.GenderId != null && filterUsers.CityId != null
            && filterUsers.AgeTo != null && filterUsers.AgeFrom != null)
        {
            usersResult = usersList.Where(u =>
                    u.Age >= filterUsers.AgeTo && u.Age <= filterUsers.AgeFrom && filterUsers.CityId == u.CityId && filterUsers.GenderId == u.CityId)
                .ToList();
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filterUsers.PageNumber, filterUsers.PageSize, filterUsers.CityId,
                filterUsers.GenderId,  filterUsers.Year, filterUsers.Month, filterUsers.Date, filterUsers.AgeTo, filterUsers.AgeFrom);
            var pagedData = usersResult.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();
            var totalRecords = await _context.Users.CountAsync();
            
            var pagedReponse =
                PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }
        
        // Только Ник нейм
        if (userName != null && filterUsers.GenderId == null && filterUsers.CityId == null
            && filterUsers.AgeTo == null && filterUsers.AgeFrom == null)
        {
            usersResult = usersList.Where(u => u.Username == userName).ToList();
            return Ok(usersResult);
        }
        
        return BadRequest("Пользователь не найден");
    }

}