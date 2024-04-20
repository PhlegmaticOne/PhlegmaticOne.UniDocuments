using UniDocuments.Text.Domain.Services.Matching.Models;
using UniDocuments.Text.Domain.Services.Matching.Options;

namespace UniDocuments.Text.Domain.Services.Matching;

public interface ITextMatchingAlgorithm
{
    MatchTextResult Match(string source, string suspicious, MatchingOptions options);
}