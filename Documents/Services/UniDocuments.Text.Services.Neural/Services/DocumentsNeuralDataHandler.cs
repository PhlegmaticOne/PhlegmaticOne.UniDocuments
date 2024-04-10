﻿using Newtonsoft.Json;
using UniDocuments.Text.Domain.Services.DocumentNameMapping;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Domain.Services.SavePath;
using UniDocuments.Text.Domain.Shared;

namespace UniDocuments.Text.Services.Neural.Services;

public class DocumentsNeuralDataHandler : IDocumentsNeuralDataHandler
{
    private const string SaveFileName = "documents_mapping.json";
    
    private readonly ISavePathProvider _savePathProvider;
    private readonly IDocumentToNameMapper _documentToNameMapper;

    private Dictionary<int, ParagraphNeuralSaveData> _paragraphsToDocumentsMap;

    public DocumentsNeuralDataHandler(ISavePathProvider savePathProvider, IDocumentToNameMapper documentToNameMapper)
    {
        _savePathProvider = savePathProvider;
        _documentToNameMapper = documentToNameMapper;
        _paragraphsToDocumentsMap = new Dictionary<int, ParagraphNeuralSaveData>();
    }

    public async Task LoadAsync()
    {
        var json = await File.ReadAllTextAsync(_savePathProvider.SavePath);
        _paragraphsToDocumentsMap = JsonConvert.DeserializeObject<Dictionary<int, ParagraphNeuralSaveData>>(json)!;
    }

    public ParagraphNeuralSaveData GetSaveData(int id)
    {
        var data = _paragraphsToDocumentsMap[id];
        data.DocumentName = _documentToNameMapper.GetDocumentName(data.DocumentId);
        return data;
    }

    public void OnTrainDataSetup(RawDocument document, RawParagraph paragraph)
    {
        _paragraphsToDocumentsMap.TryAdd(paragraph.Id, new ParagraphNeuralSaveData
        {
            DocumentId = document.Id,
            OriginalId = paragraph.OriginalId
        });
    }

    public void OnTrainComplete()
    {
        var path = Path.Combine(_savePathProvider.SavePath, SaveFileName);
        var json = JsonConvert.SerializeObject(_paragraphsToDocumentsMap);
        File.WriteAllText(path, json);
    }
}