namespace UniDocuments.Text.Domain.Services.Neural.Vocab;

public interface IDocumentsVocabProvider
{
    bool IsLoaded { get; }
    Task<DocumentVocabData> BuildAsync(CancellationToken cancellationToken);
    Task LoadAsync(CancellationToken cancellationToken);
    object GetVocab();
}