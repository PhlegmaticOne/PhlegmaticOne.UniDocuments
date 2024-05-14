using UniDocuments.Text.Domain.Providers.Comparing.Responses;

namespace UniDocuments.Text.Domain.Providers.Comparing;

public interface ITextCompareProvider
{
    CompareTextResult Compare(string a, string b, string metric);
}