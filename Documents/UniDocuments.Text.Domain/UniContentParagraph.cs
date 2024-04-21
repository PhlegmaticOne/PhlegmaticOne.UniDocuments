namespace UniDocuments.Text.Domain;

public class UniContentParagraph
{
    public string Content { get; }

    public UniContentParagraph(string content)
    {
        Content = content;
    }
    
    public override string ToString()
    {
        return Content;
    }
}