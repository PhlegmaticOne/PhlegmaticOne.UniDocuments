namespace UniDocuments.Text.Domain;

public class UniDocumentContent
{
    private const char Space = ' ';
    
    public List<UniContentParagraph> Paragraphs { get; }

    public int ParagraphsCount => Paragraphs.Count;

    public static UniDocumentContent FromString(string value)
    {
        var result = new UniDocumentContent();
        result.AddParagraph(value);
        return result;
    }

    public UniDocumentContent()
    {
        Paragraphs = new List<UniContentParagraph>();
    }

    public void AddParagraph(string content)
    {
        var id = Paragraphs.Count;
        Paragraphs.Add(new UniContentParagraph(id, content));
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