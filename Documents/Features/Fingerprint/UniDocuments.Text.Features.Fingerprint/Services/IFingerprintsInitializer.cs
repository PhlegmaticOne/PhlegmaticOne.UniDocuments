namespace UniDocuments.Text.Features.Fingerprint.Services;

public interface IFingerprintsInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken);
}