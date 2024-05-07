namespace UniDocuments.Text.Domain.Services.Neural;

public interface IDocumentTextPreprocessor
{
    Task<string> Preprocess(string text, CancellationToken cancellationToken);
}