using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.Fingerprinting.Options;

namespace UniDocuments.Text.Services.Fingerprinting.Options;

public class FingerprintOptionsProvider : IFingerprintOptionsProvider
{
    private FingerprintOptions _options;

    public FingerprintOptionsProvider(IOptionsMonitor<FingerprintOptions> options)
    {
        _options = options.CurrentValue;

        options.OnChange(newOptions =>
        {
            _options = newOptions;
        });
    }
    
    public FingerprintOptions GetOptions()
    {
        return _options;
    }
}