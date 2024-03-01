﻿using PhlegmaticOne.UniDocuments.Documents.Core;
using PhlegmaticOne.UniDocuments.Documents.Core.Algorithms;
using PhlegmaticOne.UniDocuments.Documents.Core.Features.Content;
using PhlegmaticOne.UniDocuments.Math;
using PhlegmaticOne.UniDocuments.TextProcessing.Base;
using PhlegmaticOne.UniDocuments.TextProcessing.Helpers;
using PhlegmaticOne.UniDocuments.TextProcessing.Models;

namespace PhlegmaticOne.UniDocuments.Documents.Algorithms.Cosine;

public class PlagiarismAlgorithmCosineSimilarity : IPlagiarismAlgorithm
{
    private readonly ITextPreprocessor _textPreprocessor;

    public PlagiarismAlgorithmCosineSimilarity(ITextPreprocessor textPreprocessor)
    {
        _textPreprocessor = textPreprocessor;
    }

    public IPlagiarismResult Perform(UniDocument comparing, UniDocument original)
    {
        if (!comparing.TryGetFeature<IUniDocumentTextFeature>(out var comparingContent) ||
            !original.TryGetFeature<IUniDocumentTextFeature>(out var originalContent))
        {
            return PlagiarismResultError.FromMessage("No content provided");
        }

        var proceedOriginalText = _textPreprocessor.ProcessText(new TextInput
        {
            Text = originalContent!.GetText()
        });

        var proceedComparingText = _textPreprocessor.ProcessText(new TextInput
        {
            Text = comparingContent!.GetText()
        });

        var originalWords = WordsDictionary.CreateWordsDictionary(proceedOriginalText.Words);
        var comparingWords = WordsDictionary.CreateWordsDictionary(proceedComparingText.Words);
        var allKeys = WordsDictionary.GetAllKeys(originalWords, comparingWords);
        var originalVector = UniVector<int>.FromEnumerating(allKeys, w => originalWords.GetValueOrDefault(w, 0));
        var comparingVector = UniVector<int>.FromEnumerating(allKeys, w => comparingWords.GetValueOrDefault(w, 0));
        var metric = originalVector.Cosine(comparingVector);
        return new PlagiarismResultCosine(metric);
    }
}
