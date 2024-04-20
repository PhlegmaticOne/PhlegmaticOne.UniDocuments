namespace UniDocuments.Text.Domain.Services.DocumentMapping;

public interface IDocumentMappingInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken);
}