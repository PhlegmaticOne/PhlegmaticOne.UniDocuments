namespace UniDocuments.Text.Domain.Services.Neural.Options;

public class DocumentNeuralOptions
{
    public int ParagraphMinWordsCount { get; set; }
    public string TokenizeRegex { get; set; } = null!;
    public int VectorSize { get; set; }
    public int Epochs { get; set; }
    public float Alpha { get; set; }
    public float MinAlpha { get; set; }
    public int Dm { get; set; }
    public int WorkersCount { get; set; }
    public int MinWordsCount { get; set; }
}