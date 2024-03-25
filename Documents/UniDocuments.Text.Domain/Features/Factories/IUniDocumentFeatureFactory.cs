namespace UniDocuments.Text.Domain.Features.Factories;

public interface IUniDocumentFeatureFactory
{
    UniDocumentFeatureFlag FeatureFlag { get; }
    Task<IUniDocumentFeature> CreateFeature(UniDocument document, CancellationToken cancellationToken);
}