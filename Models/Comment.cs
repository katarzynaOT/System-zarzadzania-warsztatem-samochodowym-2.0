using System.ComponentModel.DataAnnotations;


namespace WorkshopManager.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public ServiceOrder Order { get; set; } = null!; //komentarz nie moze byc bez zlecenia
        public DateTime CreatedDate { get; set; }
        public string? Content { get; set; }
        public string? Author { get; set; }
        public string? AuthorId { get; set; }

    }
}
