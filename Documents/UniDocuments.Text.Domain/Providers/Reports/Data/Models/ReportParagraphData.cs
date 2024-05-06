﻿using UniDocuments.Text.Domain.Providers.Comparing.Responses;
using UniDocuments.Text.Domain.Services.Matching.Models;

namespace UniDocuments.Text.Domain.Providers.Reports.Data.Models;

public class ReportParagraphData
{
    public Guid DocumentId { get; }
    public string DocumentName { get; }
    public int LocalId { get; }
    public string Content { get; set; } = null!;
    public double Similarity { get; private set; }
    public List<MatchTextEntry> MatchEntry { get; set; } = null!;

    private ReportParagraphData(Guid documentId, string documentName, int localId)
    {
        DocumentId = documentId;
        DocumentName = documentName;
        LocalId = localId;
    }

    public static ReportParagraphData WithDocumentData(Guid documentId, string documentName, int localId)
    {
        return new ReportParagraphData(documentId, documentName, localId);
    }

    public ReportParagraphData FillFromCompareResult(CompareTextResult compareTextResult)
    {
        Similarity = compareTextResult.SimilarityValue;
        Content = compareTextResult.Text;
        MatchEntry = compareTextResult.MatchEntry!;
        return this;
    }
}