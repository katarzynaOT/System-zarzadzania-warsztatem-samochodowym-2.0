using System.ComponentModel.DataAnnotations;


namespace WorkshopManager.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Content { get; set; }
        public string? Author { get; set; }
        public string? AuthorId { get; set; }

    }
}
