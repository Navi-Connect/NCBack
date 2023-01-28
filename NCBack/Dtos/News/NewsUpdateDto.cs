using System.ComponentModel.DataAnnotations;

namespace NCBack.Dtos.News;

public class NewsUpdateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? LinkVideo { get; set; }
    [DataType(DataType.Upload)] public IFormFile? File { get; set; }
    public string? LinkWebSites { get; set; }
}