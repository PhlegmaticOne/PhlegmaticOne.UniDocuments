using UniDocuments.Text.Domain.Features;

namespace UniDocuments.Text.Features.Text;

public class UniDocumentFeatureText : IUniDocumentFeature
{
    private readonly string _text;

    public UniDocumentFeatureText(string text)
    {
        _text = text;
    }

    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureTextFlag.Instance;

    public string GetText()
    {
        return _text;   
    }
}
