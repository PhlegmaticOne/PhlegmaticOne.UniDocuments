using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Shared;

namespace UniDocuments.Text.Services.Neural.Services;

public class DocumentNeuralSourceFile : IDocumentsNeuralSource
{
    private const int Count = 99;
    private const string BasePath = @"C:\Users\lolol\Downloads\t\{0}.txt";

    private int _currentDocumentId;

    public Task InitializeAsync()
    {
        _currentDocumentId = -1;
        return Task.CompletedTask;
    }

    public async Task<RawDocument> GetNextDocumentAsync()
    {
        if (_currentDocumentId >= Count)
        {
            return RawDocument.NoData();
        }

        _currentDocumentId += 1;
        var filePath = string.Format(BasePath, _currentDocumentId);
        var content = await File.ReadAllTextAsync(filePath);

        return new RawDocument(Guid.NewGuid(), new List<RawParagraph>
        {
            new(0, content)
        });
    }

    public void Dispose()
    {
        _currentDocumentId = -1;
    }
}