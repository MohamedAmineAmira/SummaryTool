using System.ComponentModel;

namespace Gateway.Models
{
    public class Log
    {
        public int Id { get; set; }

        public Type Type { get; set; }

        public DateTime Value { get; set; }

        public Text? Text { get; set; }

        [DefaultValue(null)]
        public string? OperationName { get; set; }
    }

    public enum Type
    {
        CreatedAt, CleaningAt, CleanedAt, ProcessingAt, ProcessedAt, DoneAt, ErrorAt
    }
}
