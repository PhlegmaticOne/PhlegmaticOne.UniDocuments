using UniDocuments.App.Domain.Services;

namespace UniDocuments.App.Services;

public class TimeProvider : ITimeProvider
{
    public DateTime Now => DateTime.UtcNow;
}