using UniDocuments.Text.Core;
using UniDocuments.Text.Core.Features;
using UniDocuments.Text.Core.Features.Factories;
using UniDocuments.Text.Features.Text;
using UniDocuments.Text.Math;
using UniDocuments.Text.Processing.Preprocessing.Base;
using UniDocuments.Text.Processing.Preprocessing.Models;
using UniDocuments.Text.Processing.WordsDictionary;

namespace UniDocuments.Text.Features.TextVector;

public class UniDocumentFeatureTextVectorFactory : IUniDocumentSharedFeatureFactory
{
    private readonly ITextPreprocessor _textPreprocessor;

    public UniDocumentFeatureTextVectorFactory(ITextPreprocessor textPreprocessor)
    {
        _textPreprocessor = textPreprocessor;
    }
    
    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureTextVectorFlag.Instance;
    public Task<IUniDocumentFeature> CreateFeature(UniDocumentEntry documentEntry)
    {
        var originalWords = CreateDocumentDictionary(documentEntry.Original);
        var comparingWords = CreateDocumentDictionary(documentEntry.Comparing);
        
        var allKeys = originalWords.Merge(comparingWords);
        
        var originalVector = UniVector<int>.FromEnumerating(allKeys, w => originalWords.GetWordEntriesCount(w));
        var comparingVector = UniVector<int>.FromEnumerating(allKeys, w => comparingWords.GetWordEntriesCount(w));

        IUniDocumentFeature feature = new UniDocumentFeatureTextVector(originalVector, comparingVector);
        return Task.FromResult(feature);
    }

    private DocumentWordsDictionary CreateDocumentDictionary(UniDocument document)
    {
        var textFeature = document.GetFeature<UniDocumentFeatureText>();
        
        var proceedText = _textPreprocessor.Preprocess(new PreprocessorTextInput
        {
            Text = textFeature.GetText()
        });
        
        return new DocumentWordsDictionary(proceedText.Words);
    }
}