namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentsVocabProvider
{
    Task BuildAsync(CancellationToken cancellationToken);
    Task LoadAsync(CancellationToken cancellationToken);
    object GetVocab();
}