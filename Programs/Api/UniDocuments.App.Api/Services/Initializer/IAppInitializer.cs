namespace UniDocuments.App.Api.Services.Initializer;

public interface IAppInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken);
}