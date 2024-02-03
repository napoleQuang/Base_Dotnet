using System.ComponentModel.DataAnnotations;

namespace API.Models.Domains
{
    public class List
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? Genre { get; set; }
        public string? Content { get; set; }

    }
}
