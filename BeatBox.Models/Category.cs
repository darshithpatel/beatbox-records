using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeatBox.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public string Name { get; set; }
        [Required]
        [DisplayName("Display Order")]
        [Range(1, 50, ErrorMessage = "Albums' Display Order must be between 1 to 50.")]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
