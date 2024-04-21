namespace UniDocuments.Text.Domain.Services.Neural.Models;

public class ParagraphTrainModel
{
    public ParagraphTrainModel(int globalId, string content)
    {
        GlobalId = globalId;
        Content = content;
    }

    public int GlobalId { get; }
    public string Content { get; }
}