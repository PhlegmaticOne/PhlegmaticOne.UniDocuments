namespace UniDocuments.App.Domain.Services;

public interface ITimeProvider
{
    DateTime Now { get; }
}