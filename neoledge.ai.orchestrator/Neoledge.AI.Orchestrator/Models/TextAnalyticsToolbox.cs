using System.ComponentModel.DataAnnotations;

namespace Neoledge.AI.Orchestrator.Models
{
    public class TextAnalyticsToolbox
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<TextProcessor>? TextProcessors { get; set; }

    }
}
