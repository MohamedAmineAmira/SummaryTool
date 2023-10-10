using System.ComponentModel.DataAnnotations;

namespace Gateway.Models
{
    public class TextAnalyticsToolbox
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<SummarizerModule>? SummarizerModules { get; set; }

    }
}
