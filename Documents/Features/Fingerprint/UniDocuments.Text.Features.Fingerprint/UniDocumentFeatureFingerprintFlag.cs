using UniDocuments.Text.Core.Features;

namespace UniDocuments.Text.Features.Fingerprint;

public class UniDocumentFeatureFingerprintFlag : UniDocumentFeatureFlag
{
    public static UniDocumentFeatureFlag Instance => new UniDocumentFeatureFingerprintFlag();
    public override string Value => "Fingerprint";
    public override int SetupOrder => 1;
}