namespace UniDocuments.Text.Domain.Services.Neural.Models;

public class DocumentTrainModel
{
    public static DocumentTrainModel Empty => new(null);
    public DocumentTrainModel() : this(new List<ParagraphTrainModel>()) { }
    
    private DocumentTrainModel(List<ParagraphTrainModel>? paragraphs)
    {
        Paragraphs = paragraphs;
    }

    public bool HasData => Paragraphs is not null;
    public List<ParagraphTrainModel>? Paragraphs { get; }

    public void AddParagraph(ParagraphTrainModel paragraph)
    {
        Paragraphs!.Add(paragraph);
    }
}