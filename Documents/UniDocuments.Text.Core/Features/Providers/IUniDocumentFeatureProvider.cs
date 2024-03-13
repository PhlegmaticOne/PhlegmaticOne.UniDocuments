namespace UniDocuments.Text.Core.Features.Providers;

public interface IUniDocumentFeatureProvider
{
    Task SetupFeatures(IEnumerable<UniDocumentFeatureFlag> featureFlag, UniDocumentEntry entry);
}