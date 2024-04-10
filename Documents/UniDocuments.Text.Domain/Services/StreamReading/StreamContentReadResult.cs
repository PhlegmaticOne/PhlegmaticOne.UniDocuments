using UniDocuments.Text.Domain.Shared;

namespace UniDocuments.Text.Domain.Services.StreamReading;

public class StreamContentReadResult
{
    public List<RawParagraph> Paragraphs { get; }

    public StreamContentReadResult()
    {
        Paragraphs = new List<RawParagraph>();
    }

    public void AddParagraph(string content)
    {
        var id = Paragraphs.Count;
        Paragraphs.Add(new RawParagraph(id, content));
    }

    public string ToRawText(char separator = ' ')
    {
        return string.Join(separator, Paragraphs);
    }
}