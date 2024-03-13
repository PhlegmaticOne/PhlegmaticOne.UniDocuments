namespace UniDocuments.Text.Core.Features.Factories;

public interface IUniDocumentFeatureFactory
{
    UniDocumentFeatureFlag FeatureFlag { get; }
    Task<IUniDocumentFeature> CreateFeature(UniDocument document);
}