using UniDocuments.Text.Domain.Services.DocumentMapping;

namespace UniDocuments.Text.Services.DocumentMapping.Initializers;

public class DocumentMapperInitializerNone : IDocumentMapperInitializer
{
    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}