using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.ContentReading;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentMapping.Extensions;
using UniDocuments.Text.Domain.Services.DocumentMapping.Models;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Models;

namespace UniDocuments.Text.Services.Neural.Sources;

[UseInPython]
public class DocumentTrainDatasetSource : IDocumentsTrainDatasetSource
{
    private readonly IDocumentMapper _documentMapper;
    private readonly IDocumentLoadingProvider _documentLoadingProvider;
    private readonly CancellationToken _cancellationToken;

    public DocumentTrainDatasetSource(IDocumentMapper documentMapper, IDocumentLoadingProvider documentLoadingProvider)
    {
        _documentMapper = documentMapper;
        _documentLoadingProvider = documentLoadingProvider;
        _cancellationToken = CancellationToken.None;
    }

    public async Task<DocumentTrainModel> GetDocumentAsync(int id)
    {
        var documentData = _documentMapper.GetDocumentData(id);

        if (documentData is null)
        {
            return DocumentTrainModel.Empty;
        }

        var document = await _documentLoadingProvider.LoadAsync(documentData.Id, false, _cancellationToken);
        return CreateDocument(document!.Content, documentData);
    }

    public async Task<List<DocumentTrainModel>> GetBatchAsync(object paragraphIdsObject)
    {
        var batch = new List<DocumentTrainModel>();
        var paragraphIds = (dynamic)paragraphIdsObject;
        var loadData = (Dictionary<Guid, List<int>>)GetLoadData(paragraphIds);
        var keys = loadData.Keys.ToHashSet();
        var result = await _documentLoadingProvider.LoadAsync(keys, false, _cancellationToken);
        
        foreach (var documentKeyValue in result)
        {
            var paragraphs = loadData[documentKeyValue.Key];
            var content = documentKeyValue.Value.Content;
            var documentData = _documentMapper.GetDocumentData(documentKeyValue.Key)!;
            var documentModel = CreateDocumentWithParagraphs(content, paragraphs, documentData);
            batch.Add(documentModel);
        }
        
        return batch;
    }

    private Dictionary<Guid, List<int>> GetLoadData(dynamic paragraphIds)
    {
        var loadData = new Dictionary<Guid, List<int>>();

        foreach (var id in paragraphIds)
        {
            var paragraphId = (int)id;
            var documentId = _documentMapper.GetDocumentIdFromGlobalParagraphId(paragraphId);

            if (documentId == int.MinValue)
            {
                continue;
            }

            var documentData = _documentMapper.GetDocumentData(documentId)!;
            var documentIdGuid = documentData.Id;

            if (loadData.TryGetValue(documentIdGuid, out var loadParagraphIds))
            {
                loadParagraphIds.Add(documentData.GetLocalParagraphId(paragraphId));
            }
            else
            {
                loadData.Add(documentIdGuid, new List<int>
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