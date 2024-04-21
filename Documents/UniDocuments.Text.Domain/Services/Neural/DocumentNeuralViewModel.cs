namespace UniDocuments.Text.Domain.Services.Neural;

public class ParagraphNeuralViewModel
{
    public ParagraphNeuralViewModel(int globalId, string content)
    {
        GlobalId = globalId;
        Content = content;
    }

    public int GlobalId { get; }
    public string Content { get; }
}

public class DocumentNeuralViewModel
{
    public static DocumentNeuralViewModel Empty => new(-1, null);
    public DocumentNeuralViewModel(int documentId, List<ParagraphNeuralViewModel>? paragraphs)
    {
        Paragraphs = paragraphs;
        DocumentId = documentId;
    }
    
    public bool HasData => Paragraphs is not null;
    public int DocumentId { get; }
    public List<ParagraphNeuralViewModel>? Paragraphs { get; }
}