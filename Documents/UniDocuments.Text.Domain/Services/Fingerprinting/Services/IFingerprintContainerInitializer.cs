namespace UniDocuments.Text.Domain.Services.Fingerprinting.Services;

public interface IFingerprintContainerInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken);
}