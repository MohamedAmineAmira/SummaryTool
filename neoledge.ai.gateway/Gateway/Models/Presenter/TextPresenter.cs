namespace Gateway.Models.Presenter
{
    public class TextPresenter
    {

        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Language { get; set; }
        public string? Type { get; set; }
        public string? PlainText { get; set; }
        public int Priority { get; set; }
        public int State { get; set; }
        public string? PrepareText { get; set; } = null;
        public string? ProcessText { get; set; } = null;
    }
}
