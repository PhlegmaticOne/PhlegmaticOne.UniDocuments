namespace UniDocuments.Text.Core.Features;

public interface IUniDocumentFeatureFactory
{
    UniDocumentFeatureFlag FeatureFlag { get; }
    Task<IUniDocumentFeature> CreateFeature(Guid documentId);
}