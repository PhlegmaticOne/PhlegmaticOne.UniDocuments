using Microsoft.Extensions.Options;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public class FingerprintOptionsProvider : IFingerprintOptionsProvider
{
    private readonly IOptions<FingerprintOptions> _options;

    public FingerprintOptionsProvider(IOptions<FingerprintOptions> options)
    {
        _options = options;
    }
    
    public FingerprintOptions GetOptions()
    {
        return _options.Value;
    }
}