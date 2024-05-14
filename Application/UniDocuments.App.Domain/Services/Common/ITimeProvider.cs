namespace UniDocuments.App.Domain.Services.Common;

public interface ITimeProvider
{
    DateTime Now { get; }
}