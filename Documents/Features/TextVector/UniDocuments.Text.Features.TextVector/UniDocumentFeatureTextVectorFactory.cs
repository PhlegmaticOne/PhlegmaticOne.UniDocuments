using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Features;
using UniDocuments.Text.Domain.Features.Factories;
using UniDocuments.Text.Domain.Services.Preprocessing;
using UniDocuments.Text.Features.Text;
using UniDocuments.Text.Math;

namespace UniDocuments.Text.Features.TextVector;

public class UniDocumentFeatureTextVectorFactory : IUniDocumentSharedFeatureFactory
{
    private readonly ITextPreprocessor _textPreprocessor;

    public UniDocumentFeatureTextVectorFactory(ITextPreprocessor textPreprocessor)
    {
        _textPreprocessor = textPreprocessor;
    }
    
    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureTextVectorFlag.Instance;
    public Task<IUniDocumentFeature> CreateFeature(UniDocumentEntry documentEntry, CancellationToken cancellationToken)
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
            Text = textFeature.Text
        });
        
        return new DocumentWordsDictionary(proceedText.Words);
    }
}