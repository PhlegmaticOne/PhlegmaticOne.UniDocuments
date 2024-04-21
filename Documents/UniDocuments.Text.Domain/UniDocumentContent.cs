namespace UniDocuments.Text.Domain;

public class UniDocumentContent
{
    private const char Space = ' ';
    
    public List<string> Paragraphs { get; }

    public int ParagraphsCount => Paragraphs.Count;

    public static UniDocumentContent FromString(string value)
    {
        var result = new UniDocumentContent();
        result.AddParagraph(value);
        return result;
    }

    public UniDocumentContent()
    {
        Paragraphs = new List<string>();
    }

    public void AddParagraph(string content)
    {
        Paragraphs.Add(content);
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