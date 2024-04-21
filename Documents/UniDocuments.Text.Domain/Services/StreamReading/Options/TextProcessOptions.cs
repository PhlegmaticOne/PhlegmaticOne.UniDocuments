namespace UniDocuments.Text.Domain.Services.StreamReading.Options;

public class TextProcessOptions
{
    public int MinWordsCount { get; set; }
    public string TokenizeRegex { get; set; } = null!;
}