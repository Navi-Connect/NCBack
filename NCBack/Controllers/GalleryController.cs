using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NCBack.Controllers;

public class GalleryController : Controller
{
    private readonly IHostingEnvironment _hostingEnvironment;

    public GalleryController(IHostingEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }


    [HttpGet("images/{namePhoto}")]
    public IActionResult Get(string namePhoto)
    {
        var path = Path.Combine(_hostingEnvironment.WebRootPath, "images", $"{namePhoto}");
        var imageFileStream = System.IO.File.OpenRead(path);
        return File(imageFileStream, "image/jpeg");
    }
}