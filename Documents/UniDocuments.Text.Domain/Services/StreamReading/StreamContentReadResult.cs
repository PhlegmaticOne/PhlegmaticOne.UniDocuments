using System.Text;
using UniDocuments.Text.Domain.Shared;

namespace UniDocuments.Text.Domain.Services.StreamReading;

public class StreamContentReadResult
{
    private const char Space = ' ';
    
    public List<RawParagraph> Paragraphs { get; }

    public static StreamContentReadResult FromString(string value)
    {
        var result = new StreamContentReadResult();
        result.AddParagraph(value);
        return result;
    }

    public StreamContentReadResult()
    {
        Paragraphs = new List<RawParagraph>();
    }

    public void AddParagraph(string content)
    {
        var id = Paragraphs.Count;
        Paragraphs.Add(new RawParagraph(id, content));
    }

    public string ToRawTextWithMinLength(int minLength, char separator = Space)
    {
        var sb = new StringBuilder();

        var selectParagraphsQuery = from paragraph in Paragraphs
            let wordsCount = paragraph.GetWordsCountApproximate()
            where wordsCount >= minLength
            select paragraph;

        foreach (var paragraph in selectParagraphsQuery)
        {
            sb.Append(paragraph.Content).Append(separator);
        }

        return sb.ToString();
    }

    public string ToRawText(char separator = Space)
    {
        return string.Join(separator, Paragraphs);
    }
}