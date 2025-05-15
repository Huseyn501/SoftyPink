using System.ComponentModel.DataAnnotations;

namespace SoftyPinko.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters.")]
        public string Name { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Position cannot be longer than 15 characters.")]
        public string Position { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Text cannot be longer than 100 characters.")]
        public string Text { get; set; }
        public string? ImgUrl { get; set; }
    }
}
