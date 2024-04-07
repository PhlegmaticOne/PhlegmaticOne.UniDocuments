namespace UniDocuments.App.Api.Extensions;

public static class TaskExtensions
{
    public static async void Forget(this Task task, ILogger? logger = null)
    {
        try
        {
            await task;
        }
        catch (Exception e)
        {
            logger?.Log(LogLevel.Critical, e.Message);
        }
    }
}