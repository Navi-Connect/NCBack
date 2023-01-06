using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCBack.Data;
using NCBack.Dtos.News;
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

    public NewsController(DataContext context, IHostEnvironment environment, UploadFileService uploadFileService)
    {
        _context = context;
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
    public async Task<ActionResult<List<News>>> GetListNews()
    {
        try
        {
            var news = await _context.News.ToListAsync();
            return Ok(news);
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
                string path = Path.Combine(_environment.ContentRootPath, "wwwroot/images/");
                string photoPath = $"images/{request.File.FileName}";
                _uploadFileService.Upload(path, request.File.FileName, request.File);

                news.Name = request.Name;
                news.Description = request.Description;
                news.Photo = photoPath;
                news.Data = DateTime.Now;
            }
            else
                news = new News()
                {
                    Name = request.Name,
                    Description = request.Description,
                    LinkVideo = request.LinkVideo,
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
                string path = Path.Combine(_environment.ContentRootPath, "wwwroot/images/");
                string? photoPath = $"images/{request.File.FileName}";
                _uploadFileService.Upload(path, request.File.FileName, request.File);

                news.Name = request.Name;
                news.Description = request.Description;
                news.Photo = photoPath;
                news.Data = DateTime.Now;
            }
            else
                news.Name = request.Name;

            news.Description = request.Description;
            news.LinkVideo = request.LinkVideo;
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