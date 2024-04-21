using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentMapping.Models;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Models;

namespace UniDocuments.Text.Services.Neural.Sources;

public class DocumentTrainDatasetSource : IDocumentsTrainDatasetSource
{
    private readonly IDocumentMapper _documentMapper;
    private readonly IDocumentLoadingProvider _documentLoadingProvider;

    private CancellationToken _cancellationToken;
    private int _currentIndex;

    public DocumentTrainDatasetSource(IDocumentMapper documentMapper, IDocumentLoadingProvider documentLoadingProvider)
    {
        _documentMapper = documentMapper;
        _documentLoadingProvider = documentLoadingProvider;
    }
    
    public void Initialize()
    {
        _currentIndex = 0;
        _cancellationToken = CancellationToken.None;
    }

    public async Task<DocumentTrainModel> GetNextDocumentAsync()
    {
        var documentData = _documentMapper.GetDocumentData(_currentIndex);

        if (documentData is null)
        {
            return DocumentTrainModel.Empty;
        }

        _currentIndex += 1;
        var document = await _documentLoadingProvider.LoadAsync(documentData.Id, false, _cancellationToken);
        return CreateDocument(document.Content!, documentData);
    }

    public async Task<List<DocumentTrainModel>> GetBatchAsync(object paragraphIdsObject)
    {
        var batch = new List<DocumentTrainModel>();
        var paragraphIds = (dynamic)paragraphIdsObject;
        var loadData = (Dictionary<int, List<int>>)GetLoadData(paragraphIds);

        foreach (var data in loadData)
        {
            var documentData = _documentMapper.GetDocumentData(data.Key)!;
            var document = await _documentLoadingProvider.LoadAsync(documentData.Id, false, _cancellationToken);
            var documentModel = CreateDocumentWithParagraphs(document.Content!, data.Value, documentData);
            batch.Add(documentModel);
        }
        
        return batch;
    }

    public void Dispose()
    {
        _currentIndex = 0;
    }

    private Dictionary<int, List<int>> GetLoadData(dynamic paragraphIds)
    {
        var loadData = new Dictionary<int, List<int>>();

        foreach (var id in paragraphIds)
        {
            var paragraphId = (int)id;
            var documentId = _documentMapper.GetDocumentIdFromGlobalParagraphId(paragraphId);

            if (documentId == int.MinValue)
            {
                continue;
            }

            var documentData = _documentMapper.GetDocumentData(documentId)!;

            if (loadData.TryGetValue(documentId, out var loadParagraphIds))
            {
                loadParagraphIds.Add(documentData.GetLocalParagraphId(paragraphId));
            }
            else
            {
                loadData.Add(documentId, new List<int>
                {
                    documentData.GetLocalParagraphId(paragraphId)
                });
            }
        }

        return loadData;
    }
    
    private static DocumentTrainModel CreateDocumentWithParagraphs(
        UniDocumentContent content, List<int> paragraphs, DocumentGlobalMapData documentData)
    {
        var document = new DocumentTrainModel();

        foreach (var paragraphId in paragraphs)
        {
            var paragraph = content.Paragraphs[paragraphId];
            var globalId = documentData.GlobalFirstParagraphId + paragraphId;
            document.AddParagraph(new ParagraphTrainModel(globalId, paragraph));
        }
        
        return document;
    }

    private static DocumentTrainModel CreateDocument(UniDocumentContent content, DocumentGlobalMapData documentData)
    {
        var document = new DocumentTrainModel();

        for (var i = 0; i < content.Paragraphs.Count; i++)
        {
            var paragraph = content.Paragraphs[i];
            var globalId = documentData.GlobalFirstParagraphId + i;
            document.AddParagraph(new ParagraphTrainModel(globalId, paragraph));
        }

        return document;
    }
}