using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.BaseMetrics.Options;

namespace UniDocuments.Text.Services.BaseMetrics.Options;

public class MetricBaselinesOptionsProvider : IMetricBaselinesOptionsProvider
{
    private MetricBaselines _options;

    public MetricBaselinesOptionsProvider(IOptionsMonitor<MetricBaselines> options)
    {
        _options = options.CurrentValue;
        options.OnChange(o => _options = o);
    }
    
    public MetricBaselines GetOptions()
    {
        return _options;
    }
}