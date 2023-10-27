using System.ComponentModel.DataAnnotations;

namespace Neoledge.AI.Orchestrator.Models
{
    public class Text
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Language { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public string? PlainText { get; set; }

        [Required]
        public int Priority { get; set; }

        [Required]

        public State State { get; set; }

        public string? PrepareText { get; set; } = null;

        public string? ProcessText { get; set; } = null;

    }
    public enum State
    {
        Created, Cleaning, Cleaned, Processing, Processed, Done, Error
    }
}
