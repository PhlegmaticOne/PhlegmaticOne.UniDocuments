using UniDocuments.Text.Domain.Features;

namespace UniDocuments.Text.Features.Text;

public class UniDocumentFeatureText : IUniDocumentFeature
{
    public UniDocumentFeatureText(string text)
    {
        Text = text;
    }
    
    public string Text { get; }

    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureTextFlag.Instance;
}
