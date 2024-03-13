namespace UniDocuments.Text.Core.Features.Factories;

public interface IUniDocumentSharedFeatureFactory
{
    UniDocumentFeatureFlag FeatureFlag { get; }
    Task<IUniDocumentFeature> CreateFeature(UniDocumentEntry documentEntry);
}