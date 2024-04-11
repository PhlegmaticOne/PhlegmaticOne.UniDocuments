using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.Neural.Options;

namespace UniDocuments.Text.Services.Neural.Services;

public class DocumentNeuralOptionsProvider : IDocumentNeuralOptionsProvider
{
    private readonly IOptionsMonitor<DocumentNeuralOptions> _optionsSnapshot;

    public DocumentNeuralOptionsProvider(IOptionsMonitor<DocumentNeuralOptions> optionsSnapshot)
    {
        _optionsSnapshot = optionsSnapshot;
    }
    
    public DocumentNeuralOptions GetOptions()
    {
        return _optionsSnapshot.CurrentValue;
    }
}