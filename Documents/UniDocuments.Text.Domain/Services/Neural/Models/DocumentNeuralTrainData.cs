namespace UniDocuments.Text.Domain.Services.Neural.Models;

public record DocumentNeuralTrainData(Guid Id, List<DocumentNeuralParagraph>? Paragraphs)
{
    public bool HasData => Paragraphs is not null;
    
    public static DocumentNeuralTrainData NoData()
    {
        return new DocumentNeuralTrainData(Guid.Empty, null);
    }
}