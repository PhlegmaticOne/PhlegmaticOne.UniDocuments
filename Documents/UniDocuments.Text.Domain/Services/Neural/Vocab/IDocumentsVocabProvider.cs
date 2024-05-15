namespace UniDocuments.Text.Domain.Services.Neural.Vocab;

public interface IDocumentsVocabProvider
{
    bool IsLoaded { get; }
    Task<DocumentVocabData> BuildAsync(IDocumentsTrainDatasetSource source);
    Task LoadAsync();
    object GetVocab();
}