using UniDocuments.App.Domain.Services.Common;

namespace UniDocuments.App.Services.Common;

public class TimeProvider : ITimeProvider
{
    public DateTime Now => DateTime.UtcNow;
}