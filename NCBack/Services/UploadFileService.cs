namespace NCBack.Services;

public class UploadFileService
{
    public async void Upload(string path, string fileName, IFormFile file)
    {
        Console.WriteLine(path);
        Console.WriteLine(fileName);
        Console.WriteLine(Path.Combine(path, fileName));
        var stream = File.Create("fsa.txt");
        await file.CopyToAsync(stream);
    }
}