using UniDocuments.Text.Domain.Features;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.Text.Features.Text;

public class UniDocumentFeatureText : IUniDocumentFeature
{
    public UniDocumentFeatureText(StreamContentReadResult content)
    {
        Content = content;
    }
    
    public StreamContentReadResult Content { get; }

    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureTextFlag.Instance;
}
