namespace UniDocuments.Text.Domain.Services.Neural.Vocab;

[Serializable]
public class DocumentVocabData
{
    public int DocumentsCount { get; set; }
    public int VocabSize { get; set; }
    public TimeSpan BuildTime { get; set; }
}