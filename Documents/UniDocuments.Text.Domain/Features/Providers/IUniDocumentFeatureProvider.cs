namespace UniDocuments.Text.Domain.Features.Providers;

public interface IUniDocumentFeatureProvider
{
    Task SetupFeatures(IEnumerable<UniDocumentFeatureFlag> featureFlag, UniDocumentEntry entry, CancellationToken cancellationToken);
}