using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gateway.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Treatment { get; set; }

        [Required]
        [ForeignKey("TextId")]
        public long TextId { get; set; }

        public Text? Text { get; set; }
    }
}
