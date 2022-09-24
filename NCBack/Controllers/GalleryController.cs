using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NCBack.Controllers;
[Route("api/[controller]")]
public class GalleryController : Controller
{
    private readonly IHostingEnvironment _hostingEnvironment;
    
    public GalleryController(IHostingEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }        

    // GET api/<controller>/5
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var path = Path.Combine(_hostingEnvironment.WebRootPath, "images", $"{id}.jpg");
        var imageFileStream = System.IO.File.OpenRead(path);
        return File(imageFileStream, "image/jpeg");
    }
}