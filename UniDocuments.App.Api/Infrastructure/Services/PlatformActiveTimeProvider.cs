using Microsoft.Extensions.Options;
using UniDocuments.App.Api.Infrastructure.Configurations;
using UniDocuments.App.Domain.Services.Common;
using UniDocuments.App.Domain.Services.Statistics;

namespace UniDocuments.App.Api.Infrastructure.Services;

public class PlatformActiveTimeProvider : IPlatformActiveTimeProvider
{
    private readonly ITimeProvider _timeProvider;
    private readonly IOptions<ApplicationConfiguration> _options;

    public PlatformActiveTimeProvider(ITimeProvider timeProvider, IOptions<ApplicationConfiguration> options)
    {
        _timeProvider = timeProvider;
        _options = options;
    }
    
    public TimeSpan GetActiveTime()
    {
        return _timeProvider.Now - _options.Value.StartTime;
    }
}