using Microsoft.AspNetCore.Mvc;

namespace NCBack.Services;

public class UploadFileService
{
    public async void Upload(string path, string fileName, IFormFile file)
    {
        Console.WriteLine(path);
        Console.WriteLine(fileName);
        Console.WriteLine(Path.Combine(path, fileName));
        await using var stream = File.Create(fileName);
        await file.CopyToAsync(stream);
    }
}