using System.ComponentModel.DataAnnotations;

namespace Gateway.Models
{
    public class DataPreprocessor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Language { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string? Url { get; set; }

    }
}
