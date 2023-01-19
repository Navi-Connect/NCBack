using System.ComponentModel.DataAnnotations;

namespace NCBack.Models
{
    public class News
    {
        public int Id { get; set; }
        public string? Name { get; set; } = String.Empty;
        public string? Description { get; set; } = String.Empty;
        public string? LinkVideo { get; set; } = String.Empty;
        public string? Photo { get; set; } = String.Empty;
        public DateTime Data { get; set; } = DateTime.Now;
    }
}

