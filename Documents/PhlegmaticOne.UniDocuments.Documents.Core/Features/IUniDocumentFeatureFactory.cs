namespace PhlegmaticOne.UniDocuments.Documents.Core.Features;

public interface IUniDocumentFeatureFactory
{
    UniDocumentFeatureFlag FeatureFlag { get; }
    Task<IUniDocumentFeature> CreateFeature(Guid documentId);
}