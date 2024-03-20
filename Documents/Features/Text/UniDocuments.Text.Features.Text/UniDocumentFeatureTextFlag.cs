using UniDocuments.Text.Domain.Features;

namespace UniDocuments.Text.Features.Text;

public class UniDocumentFeatureTextFlag : UniDocumentFeatureFlag
{
    public static UniDocumentFeatureFlag Instance => new UniDocumentFeatureTextFlag();
    public override string Value => "Text";
    public override int SetupOrder => 0;
}