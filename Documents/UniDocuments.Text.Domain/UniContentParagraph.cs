namespace UniDocuments.Text.Domain;

public class UniContentParagraph
{
    public int Id { set; get; }
    public int OriginalId { get; }
    public string Content { get; }

    public UniContentParagraph(int originalId, string content)
    {
        OriginalId = originalId;
        Content = content;
    }
    
    public override string ToString()
    {
        return Content;
    }
}