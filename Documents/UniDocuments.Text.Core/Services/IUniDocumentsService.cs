using UniDocuments.Text.Core.Features;

namespace UniDocuments.Text.Core.Services;

public interface IUniDocumentsService
{
    Task<UniDocument> GetDocumentAsync(Guid id, params UniDocumentFeatureFlag[] featureFlags);
}