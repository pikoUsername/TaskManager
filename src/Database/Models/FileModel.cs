using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models
{
    public class FileModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FileName { get; set; } = string.Empty;
        public string? MimeType { get; set; }
        public string? Size { get; set; }
        [Required]
        public string FilePath { get; set; } = string.Empty; 
    }
}