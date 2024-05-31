namespace UniDocuments.Text.Domain;

public class UniDocumentContent
{
    private const char Space = ' ';
    public IReadOnlyList<string> Paragraphs { get; }

    public int ParagraphsCount => Paragraphs.Count;

    public UniDocumentContent(IReadOnlyList<string> paragraphs)
    {
        Paragraphs = paragraphs;
    }

    public static UniDocumentContent FromString(string value)
    {
        var paragraphs = new List<string>
        {
            value
        };
        var result = new UniDocumentContent(paragraphs);
        return result;
    }
    
    public string ToRawText(char separator = Space)
    {
        return string.Join(separator, Paragraphs);
    }

    public override string ToString()
    {
        return $"Count: {ParagraphsCount}";
    }
}