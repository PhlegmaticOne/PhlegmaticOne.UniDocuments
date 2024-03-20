using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Features;
using UniDocuments.Text.Domain.Features.Factories;
using UniDocuments.Text.Features.Text.Contracts;

namespace UniDocuments.Text.Features.Text;

public class UniDocumentFeatureTextFactory : IUniDocumentFeatureFactory
{
    private readonly IDocumentTextLoader _documentTextLoader;

    public UniDocumentFeatureTextFactory(IDocumentTextLoader documentTextLoader)
    {
        _documentTextLoader = documentTextLoader;
    }
    
    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureTextFlag.Instance;
    
    public async Task<IUniDocumentFeature> CreateFeature(UniDocument document)
    {
        var text = await _documentTextLoader.LoadTextAsync(document.Id);
        return new UniDocumentFeatureText(text);
    }
}