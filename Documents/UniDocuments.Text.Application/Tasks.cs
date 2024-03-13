using UniDocuments.Text.Core;
using UniDocuments.Text.Core.Features;
using UniDocuments.Text.Core.Features.Providers;
using UniDocuments.Text.Core.Services;
using UniDocuments.Text.Plagiarism.Algorithms.Core;

namespace UniDocuments.Text.Application;

public class Tasks
{
    private readonly IUniDocumentsService _uniDocumentsService;
    private readonly IUniDocumentFeatureProvider _featureProvider;
    private readonly Dictionary<PlagiarismAlgorithmFeatureFlag, IPlagiarismAlgorithm> _plagiarismAlgorithms;

    public Tasks(IUniDocumentsService uniDocumentsService,
        IList<IPlagiarismAlgorithm> plagiarismAlgorithms,
        IUniDocumentFeatureProvider featureProvider)
    {
        _uniDocumentsService = uniDocumentsService;
        _featureProvider = featureProvider;
        _plagiarismAlgorithms = plagiarismAlgorithms.ToDictionary(x => x.FeatureFlag, x => x);
    }
    
    public async Task<UniDocumentsCompareResult> CompareDocuments(
        Guid comparingDocumentId, Guid originalDocumentId, IEnumerable<string> algorithms)
    {
        var result = new UniDocumentsCompareResult();
        var targetAlgorithms = GetTargetAlgorithms(algorithms);
        var features = GetRequiredFeatures(targetAlgorithms);
        var comparingDocument = await _uniDocumentsService.GetDocumentAsync(comparingDocumentId);
        var originalDocument = await _uniDocumentsService.GetDocumentAsync(originalDocumentId);
        var entry = new UniDocumentEntry(comparingDocument, originalDocument);
        await _featureProvider.SetupFeatures(features, entry);
        
        foreach (var algorithm in targetAlgorithms)
        {
            var plagiarismResult = algorithm.Perform(entry);
            result.AddResult(plagiarismResult);
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

    private static void AddResultFeatures(UniDocumentFeatureFlag featureFlag, ISet<UniDocumentFeatureFlag> featureFlags)
    {
        foreach (var requiredFeature in featureFlag.RequiredFeatures)
        {
            AddResultFeatures(requiredFeature, featureFlags);
        }

        featureFlags.Add(featureFlag);
    }
}