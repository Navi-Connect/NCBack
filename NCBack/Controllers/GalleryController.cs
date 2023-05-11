using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Text;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using DigitalOceanSpaces;
using Microsoft.Extensions.Options;
using NCBack.Models;
using NCBack.Spaces;


namespace NCBack.Controllers;

public class GalleryController : Controller
{
    /*private readonly IHostingEnvironment _hostingEnvironment;*/
    private readonly SpacesSettings _spacesSettings;
    public GalleryController(IOptions<SpacesSettings> settings)
    {
        _spacesSettings = settings.Value;
    }
    
    
    
    [HttpGet("{fileName}")]
    
    public async Task<IActionResult> GetPhoto(string fileName)
    {
        
        SpacesSettings settings = new SpacesSettings()
        {
            AccessKey = _spacesSettings.AccessKey,
            SecretKey = _spacesSettings.SecretKey,
            ServerUrl = _spacesSettings.ServerUrl
            
        };

        var credentials = new BasicAWSCredentials(settings.AccessKey, settings.SecretKey);
        var config = new AmazonS3Config
        {
            ServiceURL = settings.ServerUrl,
            ForcePathStyle = true
        };
        var s3Client = new AmazonS3Client(credentials, config);

        var request = new GetObjectRequest
        {
            BucketName = "ncback-file/ncback-file",
            Key = $"sampleimage/{fileName}"
        };
        var response = await s3Client.GetObjectAsync(request);

        return File(response.ResponseStream, response.Headers.ContentType);
    }

}