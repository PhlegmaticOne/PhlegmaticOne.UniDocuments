using UniDocuments.Text.Domain.Features;
using UniDocuments.Text.Features.Text;

namespace UniDocuments.Text.Features.TextVector;

public class UniDocumentFeatureTextVectorFlag : UniDocumentFeatureFlag
{
    public static UniDocumentFeatureFlag Instance => new UniDocumentFeatureTextVectorFlag();

    public override IEnumerable<UniDocumentFeatureFlag> RequiredFeatures
    {
        get
        {
            yield return UniDocumentFeatureTextFlag.Instance;
        }
    }
    
    public override string Value => "TextVector";
    public override bool IsShared => true;
    public override int SetupOrder => 1;
}