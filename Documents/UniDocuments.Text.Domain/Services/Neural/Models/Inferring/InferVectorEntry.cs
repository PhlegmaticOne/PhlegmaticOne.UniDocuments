﻿namespace UniDocuments.Text.Domain.Services.Neural.Models.Inferring;

public class InferVectorEntry
{
    public int ParagraphId { get; }
    public double Similarity { get; }

    public InferVectorEntry(int paragraphId, double similarity)
    {
        ParagraphId = paragraphId;
        Similarity = similarity;
    }
}