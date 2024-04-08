using UniDocuments.Text.Domain.Services.Common;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.Text.Services.Neural.Services;

public class DocumentNeuralDataHandler : IDocumentNeuralDataHandler
{
    private struct Data
    {
        public int OriginalId;
        public Guid DocumentId;
    }

    private readonly Dictionary<int, Data> _paragraphsToDocumentsMap = new();
    
    public void OnTrainDataSetup(RawDocument document, RawParagraph paragraph)
    {
        _paragraphsToDocumentsMap.TryAdd(paragraph.Id, new Data
        {
            DocumentId = document.Id,
            OriginalId = paragraph.OriginalId
        });
    }
}