namespace UniDocuments.Text.Domain.Services.DocumentMapping;

public interface IDocumentMapperInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken);
}