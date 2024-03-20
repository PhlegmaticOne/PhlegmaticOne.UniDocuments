namespace UniDocuments.Text.Domain.Features.Factories;

public interface IUniDocumentSharedFeatureFactory
{
    UniDocumentFeatureFlag FeatureFlag { get; }
    Task<IUniDocumentFeature> CreateFeature(UniDocumentEntry documentEntry);
}