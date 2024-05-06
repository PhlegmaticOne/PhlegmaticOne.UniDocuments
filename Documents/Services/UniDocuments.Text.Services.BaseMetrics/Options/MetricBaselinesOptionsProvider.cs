using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.BaseMetrics.Options;

namespace UniDocuments.Text.Services.BaseMetrics.Options;

public class MetricBaselinesOptionsProvider : IMetricBaselinesOptionsProvider
{
    private readonly IOptions<MetricBaselines> _options;

    public MetricBaselinesOptionsProvider(IOptions<MetricBaselines> options)
    {
        _options = options;
    }
    
    public MetricBaselines GetOptions()
    {
        return _options.Value;
    }
}