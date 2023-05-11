using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using NCBack.Spaces;

namespace NCBack.Services;

public class UploadFileService
{
    private readonly SpacesSettings _spacesSettings;
    public UploadFileService(IOptions<SpacesSettings> settings)
    {
        _spacesSettings = settings.Value;
    }
    
    /*
    public async void Upload(string path, string fileName, IFormFile file)
    {
        using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
        await file.CopyToAsync(stream);
    }
    */
    
    public async Task<PutObjectResponse> Upload(IFormFile file)
    {
        
        SpacesSettings settings = new SpacesSettings()
        {
            AccessKey = _spacesSettings.AccessKey,
            SecretKey = _spacesSettings.SecretKey,
            Endpoint = _spacesSettings.Endpoint
        };
        
        
        var config = new AmazonS3Config
        {
            ServiceURL = settings.Endpoint,
            ForcePathStyle = true
        };
        var client = new AmazonS3Client(settings.AccessKey, settings.SecretKey, config);

        using (var stream = file.OpenReadStream())
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = "ncback-file",
                Key = $"sampleimage/{file.FileName}",
                InputStream = stream,
                ContentType = file.ContentType
            };

            var response = await client.PutObjectAsync(putRequest);
            return response;
        }
    }
    /*public async Task<bool> UploadFileToSpacesAsync(string fileName, IFormFile file)
    {
        try
        {
            var endpointUrl = $"https://{bucketName}.nyc3.digitaloceanspaces.com";

            var s3Client = new AmazonS3Client(accessKey, secretKey, new AmazonS3Config
            {
                ServiceURL = endpointUrl,
                ForcePathStyle = true
            });

            // Читаем поток данных из IFormFile
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            // Загружаем файл в Spaces
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileName,
                InputStream = stream,
                ContentType = file.ContentType
            };

            var response = await s3Client.PutObjectAsync(putRequest);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch
        {
            return false;
        }
    }*/
    
}