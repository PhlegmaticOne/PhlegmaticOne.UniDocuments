namespace UniDocuments.App.Domain.Services.Statistics;

public interface IPlatformActiveTimeProvider
{
    TimeSpan GetActiveTime();
}