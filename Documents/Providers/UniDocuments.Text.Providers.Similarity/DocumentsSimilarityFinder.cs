using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Algorithms;
using UniDocuments.Text.Domain.Features;
using UniDocuments.Text.Domain.Features.Providers;
using UniDocuments.Text.Domain.Providers.Similarity;
using UniDocuments.Text.Domain.Providers.Similarity.Requests;
using UniDocuments.Text.Domain.Providers.Similarity.Responses;
using UniDocuments.Text.Domain.Services.Documents;
using UniDocuments.Text.Features.Text;

namespace UniDocuments.Text.Providers.Similarity;

public class DocumentsSimilarityFinder : IDocumentsSimilarityFinder
{
    private readonly IUniDocumentsService _uniDocumentsService;
    private readonly IUniDocumentFeatureProvider _featureProvider;
    private readonly Dictionary<PlagiarismAlgorithmFeatureFlag, IPlagiarismAlgorithm> _plagiarismAlgorithms;

    public DocumentsSimilarityFinder(
        IUniDocumentsService uniDocumentsService,
        IEnumerable<IPlagiarismAlgorithm> plagiarismAlgorithms,
        IUniDocumentFeatureProvider featureProvider)
    {
        _uniDocumentsService = uniDocumentsService;
        _featureProvider = featureProvider;
        _plagiarismAlgorithms = plagiarismAlgorithms.ToDictionary(x => x.FeatureFlag, x => x);
    }

    public async Task<SimilarityResponse> CompareAsync(
        DocumentsSimilarityRequest request, CancellationToken cancellationToken)
    {
        var targetAlgorithms = GetTargetAlgorithms(request.Algorithms);
        var features = GetRequiredFeatures(targetAlgorithms);
        
        var comparingDocument = await _uniDocumentsService
            .GetAsync(request.ComparingDocumentId, cancellationToken);
        var originalDocument = await _uniDocumentsService
            .GetAsync(request.OriginalDocumentId, cancellationToken);
        
        var entry = new UniDocumentEntry(comparingDocument, originalDocument);
        await _featureProvider.SetupFeatures(features, entry, cancellationToken);
        return ExecuteAlgorithms(targetAlgorithms, entry);
    }

    public async Task<List<SimilarityResponse>> CompareAsync(TextsSimilarityRequest request, CancellationToken cancellationToken)
    {
        var result = new List<SimilarityResponse>();
        var targetAlgorithms = GetTargetAlgorithms(request.Algorithms);
        var features = GetRequiredFeatures(targetAlgorithms).ToArray();
        
        var document = CreateDocumentWithText(request.Text);
        
        foreach (var otherText in request.OtherTexts)
        {
            var otherDocument = CreateDocumentWithText(otherText);
            var entry = new UniDocumentEntry(otherDocument, document);
            await _featureProvider.SetupFeatures(features, entry, cancellationToken);
            var similarityResponse = ExecuteAlgorithms(targetAlgorithms, entry);
            result.Add(similarityResponse);
        }

        return result;
    }

    private IPlagiarismAlgorithm[] GetTargetAlgorithms(IEnumerable<string> algorithms)
    {
        return algorithms.Select(x => new PlagiarismAlgorithmFeatureFlag(x))
            .Where(x => _plagiarismAlgorithms.ContainsKey(x))
            .Select(x => _plagiarismAlgorithms[x])
            .ToArray();
    }

    private static UniDocument CreateDocumentWithText(string text)
    {
        return UniDocument.Empty.WithFeature(UniDocumentFeatureText.FromString(text));
    }

    private static IEnumerable<UniDocumentFeatureFlag> GetRequiredFeatures(IEnumerable<IPlagiarismAlgorithm> algorithms)
    {
        var set = new HashSet<UniDocumentFeatureFlag>();

        foreach (var algorithm in algorithms)
        {
            foreach (var featureFlag in algorithm.GetRequiredFeatures())
            {
                AddResultFeatures(featureFlag, set);
            }
        }

        return set;
    }

    private static SimilarityResponse ExecuteAlgorithms(IPlagiarismAlgorithm[] targetAlgorithms, UniDocumentEntry entry)
    {
        var result = new SimilarityResponse();
        
        foreach (var algorithm in targetAlgorithms)
        {
            var plagiarismResult = algorithm.Perform(entry);
            result.AddResult(plagiarismResult);
        }

        return result;
    }

    private static void AddResultFeatures(UniDocumentFeatureFlag featureFlag, ISet<UniDocumentFeatureFlag> featureFlags)
    {
        foreach (var requiredFeature in featureFlag.RequiredFeatures)
        {
            AddResultFeatures(requiredFeature, featureFlags);
        }

        featureFlags.Add(featureFlag);
    }
}