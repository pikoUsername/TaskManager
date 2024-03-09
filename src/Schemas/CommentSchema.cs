using System.ComponentModel.DataAnnotations;

namespace TaskManager.Schemas
{
    public class CreateCommentScheme
    {
        [Required]
        public Guid TaskId { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty; 
    }

    public class GetCommentsScheme
    {
        public Guid TaskId { get; set; }
        public Guid? UserId { get; set; } 
        public int End { get; set; } = 10;
        public int Start { get; set; } = 0; 
    }

    public class UpdateCommentSchema
    {
        public string Text { get; set; }
    } 
}
