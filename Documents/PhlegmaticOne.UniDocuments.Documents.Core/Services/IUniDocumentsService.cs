using PhlegmaticOne.UniDocuments.Documents.Core.Features;

namespace PhlegmaticOne.UniDocuments.Documents.Core.Services;

public interface IUniDocumentsService
{
    Task<UniDocument> GetDocumentAsync(Guid id, params UniDocumentFeatureFlag[] featureFlags);
}