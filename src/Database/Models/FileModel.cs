using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models
{
    public class FileModel
    {
        [Key]
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string? MimeType { get; set; }
        public string? Size { get; set; }
        public string FilePath { get; set; }
    }
}
