﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;
using NCBack.Dtos.News;
using NCBack.Filter;
using NCBack.Helpers;
using NCBack.Models;
using NCBack.Services;

namespace NCBack.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsController : Controller
{
    private readonly DataContext _context;
    private readonly IHostEnvironment _environment; //Добавляем сервис взаимодействия с файлами в рамках хоста
    private readonly UploadFileService _uploadFileService; // Добавляем сервис для получения файлов из формы
    private readonly IUriService _uriServiceNews;

    public NewsController(DataContext context, IHostEnvironment environment, UploadFileService uploadFileService,
        IUriService uriServiceNews)
    {
        _context = context;
        _uriServiceNews = uriServiceNews;
        _environment = environment;
        _uploadFileService = uploadFileService;
    }

    [HttpGet("getNewsById")]
    public async Task<IActionResult> GetNewsById(int id)
    {
        try
        {
            var news = await _context.News.FirstOrDefaultAsync(n => n.Id == id);
            if (news != null) return Ok(news);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("System errors !!!");
    }

    [HttpGet("listNews")]
    public async Task<ActionResult<List<News>>> GetListNews([FromQuery] ObjectPaginationFilter? filter)
    {
        try
        {
            var news = await _context.News.Distinct().OrderByDescending(n => n.Id).ToListAsync();
           // PaginationHelper.ReversEventList(news);
            var route = Request.Path.Value;
            var validFilter = new ObjectPaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = news
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = news.Count();
            var pagedReponse =
                PaginationHelper.CreatePagedObjectReponse(pagedData, validFilter, totalRecords, _uriServiceNews, route);
            return Ok(pagedReponse);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("System errors !!!");
    }

    [HttpPost("createNewsPhoto")]
    public async Task<ActionResult<List<News>>> CreateNewsPhoto([FromForm] NewsCreateDto request)
    {
        try
        {
            News news = new News();
            if (request.File != null)
            {
               // string path = Path.Combine(_environment.ContentRootPath, "wwwroot/images/");
                string photoPath = $"sampleimage/{request.File.FileName}";
                await _uploadFileService.Upload(request.File);

                news.Name = request.Name;
                news.Description = request.Description;
                news.Photo = photoPath;
                news.LinkWebSites = request.LinkWebSites;
                news.Data = DateTime.Now;
            }
            else
                news = new News()
                {
                    Name = request.Name,
                    Description = request.Description,
                    LinkVideo = request.LinkVideo,
                    LinkWebSites = request.LinkWebSites,
                    Data = DateTime.Now
                };

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return Ok(news);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("System errors !!!");
    }

    [HttpPut("updateNews/{Id}")]
    public async Task<ActionResult<List<News>>> UpdateNews([FromForm] NewsUpdateDto request, int? Id)
    {
        try
        {
            var news = _context.News.FirstOrDefault(n => n.Id == Id);
            if (news == null)
                return BadRequest("News not found.");

            if (request.File != null)
            {
                //string path = Path.Combine(_environment.ContentRootPath, "wwwroot/images/");
                string photoPath = $"sampleimage/{request.File.FileName}";
                await _uploadFileService.Upload(request.File);

                news.Name = request.Name;
                news.Description = request.Description;
                news.Photo = photoPath;
                news.LinkWebSites = request.LinkWebSites;
                news.Data = DateTime.Now;
            }
            else
                news.Name = request.Name;

            news.Description = request.Description;
            news.LinkVideo = request.LinkVideo;
            news.LinkWebSites = request.LinkWebSites;
            news.Data = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(news);
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("System errors !!!");
    }

    [HttpDelete("deleteNews/{id}")]
    public async Task<ActionResult<List<News>>> DeleteNews(int id)
    {
        try
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
                await _context.SaveChangesAsync();
                return Ok(news);
            }
        }
        catch (ApplicationException e)
        {
            throw new ApplicationException(e.ToString());
        }

        return BadRequest("System errors !!!");
    }
}