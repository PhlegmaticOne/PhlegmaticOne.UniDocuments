namespace UniDocuments.Text.Domain.Shared;

public class RawParagraph
{
    public int Id { set; get; }
    public int OriginalId { get; }
    public string Content { get; }

    public RawParagraph(int originalId, string content)
    {
        OriginalId = originalId;
        Content = content;
    }
    
    public override string ToString()
    {
        return Content;
    }
}